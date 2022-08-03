# cauchy-problem-numeric-calculator

The program solves differential equations of the following form:

    y'(x) = f(x, y),
    y(x0) = y0.
    
    x0 - first point of selected range, y0 = y(x0)
    
The f(x) input is going according to the rules for entering mathematical expressions in C#. Аt the moment input supports only arithmetic symbols and brackets. Input example:

    -2 * (y + 1) / x
    
DelegateConstructor class was written by Deynega Vasiliy

The original of his article: http://itw66.ru/blog/c_sharp/662.html

.NET Framework 4.5 used.
