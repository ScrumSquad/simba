var gulp = require('gulp'),
    elm  = require('gulp-elm'),
    stylus = require('gulp-stylus'),
    watch = require('gulp-watch'),
    server = require('gulp-server-livereload')
;

gulp.task('webserver', function() {
  gulp.src('../wwwroot')
    .pipe(server({
      livereload: true,
      directoryListing: false,
      open: true
    }));
});

//***************************************
// compile source files to app dir
//***************************************

var notify = function(message) {
    console.log(message);
};

gulp.task('elm-init', elm.init);


gulp.task('elm', ['elm-init'], function(){
    return gulp.src('src/**/*.elm')
        .pipe(watch('src/**/*.elm',function(event){
            notify("Elm Changed!");
        }))
        .pipe(elm())
        .pipe(gulp.dest('../wwwroot'));
});

gulp.task('compileStylus',function(){
    return gulp.src('src/**/*.styl')
        .pipe(watch('src/**/*.styl',function(event){
            notify("Styles Changed!");
        }))
        .pipe(stylus())
        .pipe(gulp.dest('../wwwroot'))
    ;
});


// gulp.task('default',['webserver','compileStylus']);
gulp.task('default',['compileStylus','elm']);
