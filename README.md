# ResistanceCalculator

## Introduction

### Problem Statement

Create a console application dedicated to calculating the resistance of 3, 4, 5, or 6 band resistors. This is specifically targeted for students working in labs, who can then reallocate time usually spent towards calculating resistances, towards actual lab work. 

### Project Objective

The project objectives included:

1.	To gain experience with programming in the C# language, specifically practicing new skills such as enums, switch statements, and static vs instantiated classes. 
2.	To solidify general programming concepts such as functions, loops, try statements, and more importantly, error handling. 
3.	To solve a real-life problem, no matter how small, that I’ve been facing.

## Technologies

The technologies used include:

1. C# for Visual Studio Code (powered by OmniSharp) v1.23.8

## Solution

The following solution was implemented:

1.	Created a design that is user interactive in the sense that the program asks the user to input the number of bands on their resistor and the colour code of each band (following the legend displayed). It then displays the resistance and tolerance, and in the case of a 6 band resistor, also displays the temperature coefficient.
2.	Developed strong error handling structures that not only check for input error, but also validate that the input is logical (such as a particular band colour not making sense).

## Launch

In order to run this program through the MacOS terminal, run the following commands:

1. csc Program.cs
2. mono Program.exe

To launch from other operating systems, please follow respective instructions found online.

## Possible Improvements

Possible improvements for this project include:

1.	Currently, resistance values are limited by the floating-point type (resistance is stored as a float). While the resistance is limited at an impractical value that students will not come across, coding to eliminate this problem will be a good learning practice. 
2.	Create a form for the program, rather than have it as a console application, in which users can physically select the colors and then visually see the resistor that they have. 
3.	Explore how nested classes/interfaces can be implemented to improve the code, in order to facilitate my learning of C#. 
