module.exports = function (grunt) {
    'use strict';

    const nodeSass = require('node-sass');

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        // Sass
        sass: {
            options: {
                sourceMap: false, // Create source map
                outputStyle: 'compressed', // Minify output
                implementation: nodeSass
            },
            dist: {
                files: [
                    {
                        expand: true, // Recursive
                        cwd: "styles", // The startup directory
                        src: ["**/*.scss"], // Source files
                        dest: "wwwroot/styles", // Destination
                        ext: ".css" // File extension
                    },
                    {
                        expand: true, // Recursive
                        cwd: "Pages", // The startup directory
                        src: ["**/*.scss"], // Source files
                        dest: "wwwroot/pages", // Destination
                        ext: ".css" // File extension
                    }
                ]
            }
        }
    });

    // Load the plugin
    grunt.loadNpmTasks('grunt-sass');

    // Default task(s).
    grunt.registerTask('default', ['sass']);
};