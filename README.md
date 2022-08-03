# cauchy-problem-numeric-calculator
This C# WPF application allows calculate Cauchy problem solution at the last point of selected range.

The program solves differential equations of the following form:

    y'(x) = f(x, y),
    y(x0) = y0.
    
    x0 - first point of selected range, y0 = y(x0)
    
The f(x) input is going according to the rules for entering mathematical expressions in C#.

DelegateConstructor class was written by Deynega Vasiliy

The original of his article: http://itw66.ru/blog/c_sharp/662.html
