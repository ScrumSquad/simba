module Main exposing (..)

import Json.Decode as Decode exposing (string, field, int)
import Html.Attributes exposing (..)
import Array exposing (Array)
import Http exposing (..)
import Html exposing (..)
import Html.Events exposing (onInput, onClick)
import List exposing (length)


main : Program Never Model Msg
main =
    Html.program
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
        }


init : ( Model, Cmd Msg )
init =
    ( initialModel, getProcesses )



-- MODEL


type alias Model =
    { processes : List Process
    , filter : String
    , remaining : List Process
    , to_kill : Maybe Process
    }


type alias Process =
    { id : Int
    , name : String
    , memory : Int
    }


initialModel : Model
initialModel =
    Model [] "" [] Nothing


process_decoder : Decode.Decoder Process
process_decoder =
    Decode.map3 Process (field "processID" int) (field "processName" string) (field "memory" int)


process_list_decoder : Decode.Decoder (Array Process)
process_list_decoder =
    Decode.array process_decoder



-- MESSAGES


type Msg
    = GetProcesses
    | Processes (Result Http.Error (Array Process))
    | ProcessKilled (Result Http.Error () )
    | Filter String
    | ConfirmKill Process
    | Cancel
    | Kill Process



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GetProcesses ->
            ( model, getProcesses )

        Filter str ->
            let
                remaining =
                    List.filter (\x -> String.contains str x.name) model.processes
            in
                ( { model | filter = str, remaining = remaining }, Cmd.none )

        ConfirmKill process ->
            ( { model | to_kill = Just process }, Cmd.none )

        Kill process ->
            ( { model | to_kill = Nothing }, sendKill process )

        Cancel ->
            ( { model | to_kill = Nothing }, Cmd.none )

        Processes (Ok res) ->
            let
                processes =
                    Array.toList res

                remaining =
                    List.filter (\x -> String.startsWith model.filter x.name) processes
            in
                ( { model | processes = processes, remaining = remaining }, Cmd.none )

        Processes (Err value) ->
            ( model, Cmd.none )

        ProcessKilled _ ->
            (model, getProcesses)

delete_request : String  -> Request ()
delete_request url =
  request
    { method = "DELETE"
    , headers = []
    , url = url
    , body = emptyBody
    , expect = expectStringResponse (\_ -> Ok ())
    , timeout = Nothing
    , withCredentials = False
    }


sendKill : Process -> Cmd Msg
sendKill process =
    let
        url =
            "http://localhost:5000/api/Processes/" ++ toString process.id
    in
        Http.send ProcessKilled <|
            delete_request url


getProcesses : Cmd Msg
getProcesses =
    let
        url =
            "http://localhost:5000/api/Processes"
    in
        Http.send Processes <|
            Http.get url process_list_decoder



-- VIEW


show_process : Process -> Html Msg
show_process process =
    div [ class "process" ]
        [ div [ class "indicator" ] [ text ">" ]
        , div [ class "process_name" ] [ text process.name ]
        , div [ class "process_id" ] [ text <| toString process.id ]
        , div [ class "process_memory" ] [ text <| toString process.memory ]
        , div [ class "close_button", onClick <| ConfirmKill process ] [ text "X" ]
        ]


process_count : List Process -> String
process_count processes =
    toString <| length processes


filtered_count : List Process -> String
filtered_count processes =
    toString <| length processes


filter_label : Model -> Html Msg
filter_label model =
    div [ class "filter_label" ] [ text <| "Filter Processes (" ++ filtered_count model.remaining ++ "/" ++ process_count model.processes ++ ") " ]


header : Model -> Html Msg
header model =
    div [ class "header_container" ]
        [ filter_label model
        , input [ placeholder "filter", class "filter_input", onInput Filter ] []
        ]


process_list : Model -> Html Msg
process_list model =
    div [ class "process_list" ] <| List.map show_process model.remaining


confirmation : Process -> Html Msg
confirmation process =
    div [ class "confirmation_container", onClick Cancel ]
        [ div [ class "dialog" ]
            [ div [ class "dialog_header" ] [ text "This process has a family :( " ]
            , div [ class "dialog_prompt" ]
                [ text <| "Are you sure you want to kill "
                , span [ class "dialog_processname" ] [ text process.name ]
                , text "  ?"
                ]
            , div [ class "dialog_buttons" ]
                [ div [ class "cancel", onClick Cancel ] [ text "Cancel" ]
                , div [ class "submit", onClick <| Kill process ] [ text "I am a murdered" ]
                ]
            ]
        ]


view : Model -> Html Msg
view model =
    div [ class "main_container" ]
        [ case model.to_kill of
            Nothing ->
                text ""

            Just process ->
                confirmation process
        , header model
        , process_list model
        ]



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none
