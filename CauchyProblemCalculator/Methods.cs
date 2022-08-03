using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauchyProblem
{
    class Methods
    {
        static private DelegateConstructor.functionDelegate function = DelegateConstructor.MakeDelegate(MainWindow.inputFunction);

        static private readonly float step = Math.Abs(MainWindow.rightLimit - MainWindow.leftLimit) / MainWindow.splitPointsNum;

        static public float Euler(int pointsNum, float x, float y)
        {
            for (int i = 1; i <= pointsNum; ++i)
            {
                y += step * function(x, y);
                x += step;
            }

            return y;
        }

        static public float EulerMod(int pointsNum, float x, float y)
        {
            for (int i = 1; i <= pointsNum; ++i)
            {
                y += step / 2 * (function(x, y) + function(x + step, y + step * function(x, y)));
                x += step;
            }

            return y;
        }

        static public float Cauchy(int pointsNum, float x, float y)
        {
            for (int i = 1; i <= pointsNum; ++i)
            {
                y += step * function(x + step / 2, y + (step / 2) * function(x, y));
                x += step;
            }

            return y;
        }

        static public float RungeKutta4th(int pointsNum, float x, float y)
        {
            float k1, k2, k3, k4;

            for (int i = 1; i <= pointsNum; ++i)
            {
                k1 = step * function(x, y);
                k2 = step * function(x + step / 2, y + k1 / 2);
                k3 = step * function(x + step / 2, y + k2 / 2);
                k4 = step * function(x + step, y + k3);

                y += Convert.ToSingle(1.0 / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4);

                x += step;
            }

            return y;
        }

        static public float AdamsBashforth(int pointsNum, float x, float y)
        {
            List<float> Y = new List<float>(pointsNum + 1);

            Y.Add(y);

            float k1, k2, k3, k4;

            for (int i = 1; i < 3; ++i)
            {
                k1 = step * function(x, Y[i - 1]);
                k2 = step * function(x + step / 2, Y[i - 1] + k1 / 2);
                k3 = step * function(x + step / 2, Y[i - 1] + k2 / 2);
                k4 = step * function(x + step, Y[i - 1] + k3);

                Y.Add(Y[i - 1] + Convert.ToSingle(1.0 / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4));

                x += step;
            }

            for (int i = 3; i <= pointsNum; ++i)
            {
                Y.Add(Y[i - 1] + (step / 12) * (23 * function(x, Y[i - 1]) - 16 * function(x - step, Y[i - 2]) + 5 * function(x - 2 * step, Y[i - 3])));

                x += step;
            }

            return Y[pointsNum];
        }
    }
}
