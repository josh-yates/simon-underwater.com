var gulp = require('gulp');
var sass = require('gulp-sass');
var del = require('del');
var cssMinify = require('gulp-clean-css');

function sassClean() {
    return del(['wwwroot/styles']);
}

function sassBuild() {
    return gulp.src('styles/**/*.scss')
      .pipe(sass())
      .pipe(cssMinify())
      .pipe(gulp.dest('wwwroot/styles'));
}

function sassWatch() {
    gulp.watch('styles/**/*.scss', gulp.series(sassClean, sassBuild));
}

exports.watch = gulp.parallel(sassWatch);
exports.build = gulp.series(sassClean, sassBuild);