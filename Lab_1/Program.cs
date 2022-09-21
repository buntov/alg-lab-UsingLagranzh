using System;
namespace Lab_1
{
   

    class Program
    {
       

        static void Main(string[] args)
        {
            double a_min = 0;
            double a_max = 1.5;
            double x, h = 0.01;
            double loop_step = a_max+h;//1.51
            int sizearray = Convert.ToInt32((((a_max - a_min)+h )/ h));//151
            int size_steps = Convert.ToInt32(((a_max - a_min) / h));//150
            double[] x_value = { 0.11, 0.53, 1.6, 1.23, 1.37, 1.44, 1.5, 1.57, 1.63, 1.76, 1.86, 2.16 };
            int sizearray_y = x_value.Length;//12
            double[] y_value = new double[sizearray_y];
             double[] forFunction = new double[sizearray];
             double[] forCubic = new double[sizearray];
             double[] forLagranzh = new double[sizearray];

             OriginalTable(x_value, y_value, sizearray_y);
             UsingFunction(x_value, forFunction, loop_step, h);

             int p= 0 ;
             for(x = 0; x <= loop_step; x+=h, p++)
             {
                 forCubic[p] = UsingCubic(x_value, y_value, Number(x_value, y_value, x, sizearray_y), 3, x);
             }
             UsingLagranzh(x_value, forLagranzh, loop_step, h, sizearray_y);
             Comparison(forFunction, forCubic, forLagranzh, loop_step, h);
             Console.WriteLine("\n\n");

             double[,] table2 = new double[6, sizearray];
             x = 0;
             for (int i = 0; i <= size_steps; i++, x += h)
             {
                 table2[0, i] = x;
                 table2[1, i] = forLagranzh[i];
                 table2[2, i] = Math.Exp(x) / (Math.Exp(2.0 * x) + 1);
                 table2[3, i] = Derivative1(x, h, i, forCubic, size_steps);
                 table2[4, i] = (Math.Exp(x) - Math.Exp(3 * x)) / Math.Pow(1 + Math.Exp(2 * x), 2.0);
                 table2[5, i] = Derivative2(x, h , i, forCubic, size_steps);
             }
             Console.WriteLine("x\t\tf(x)    \tf`(x)   \tf`(x)   \tf``(x)   \tf``(x)\n \t\tFunction\tDiffCalc\tFunction\tDiffCalc\n");
            for (int i = 0; i < table2.GetLength(1); i++)
            {
                for (int j = 0; j < table2.GetLength(0); j++)
                { Console.Write("{0:F5} \t", table2[j, i]); }
                Console.WriteLine();
            }
            
            // Console.WriteLine(sizem);
            Console.ReadKey();
         }

         static void OriginalTable(double[] x_value, double[] y_value, int sizearray_y) 
         {
             Console.Write("Задана таблиця значень функції має вигляд: \n   x:");

             for (int i = 0; i < sizearray_y; i++)    
             {
                 Console.Write("   {0, 4:F2}  ", x_value[i]);
             }

             Console.Write("\nf(x):");

             for (int i = 0; i < sizearray_y; i++)    
             {
                 y_value[i] = Function(x_value[i]);
                 Console.Write("   {0, 4:F2}  ", y_value[i]);
             }

             Console.WriteLine();
         }

         static void UsingFunction(double[] x_value, double[] forFunction, double loop_step, double h)
         {
             int k = 0;

             for (double x = 0.00; x <= loop_step; x += h, k++)
             {
                 forFunction[k] = Function(x);
             }
         }

         static double Function(double x)    
         {
             return Math.Atan(Math.Exp(x));
         }

         static int Number(double[] x_value, double[] y_value, double x, int sizearray_y)
         {
             int k = 0;
             for (int i = k + 1; i < sizearray_y - 2; ++i)
             {
                 if (Math.Abs(x - x_value[i]) + Math.Abs(x - x_value[i + 1]) + Math.Abs(x - x_value[ i + 2]) < Math.Abs(x - x_value[k]) + Math.Abs(x - x_value[k + 1]) + Math.Abs(x - x_value[k + 2]))
                     k = i;
                 if (Math.Abs(x - x_value[i]) + Math.Abs(x - x_value[i + 1]) + Math.Abs(x - x_value[i + 2]) > Math.Abs(x - x_value[k]) + Math.Abs(x - x_value[k + 1]) + Math.Abs(x - x_value[k + 2]))
                     break;
             }
             return k;
         }
         static double UsingCubic(double[] x_value, double[] y_value, int k, int n, double x)
         {
             double rezult = 0;

             for (int i = k; i < k + n; i++)
             {
                 double L = 1;
                 for (int j = k; j < k + n; j++)
                 {
                     if (i != j) L *= (x - x_value[j]) / (x_value[i] - x_value[j]);
                 }
                 rezult += y_value[i] * L;
             }
             return rezult;
         }

         static void UsingLagranzh(double[] x_value, double[] forLagranzh, double loop_step, double h, int sizearray_y)
         {
             int k = 0;
             double polynom = 0; 
             double promezh = 1; 
             for (double x = 0.00; x <= loop_step; x += h, k++) 
             {
                 for (int i = 0; i < sizearray_y; i++) 
                 {
                     for (int j = 0; j < sizearray_y; j++) 
                     {
                         if (j == i)
                             continue;
                         else
                             promezh = promezh * (x - x_value[j]) / (x_value[i] - x_value[j]);
                     }
                     polynom = polynom + Function(x_value[i]) * promezh;
                     promezh = 1;
                 }
                 forLagranzh[k] = polynom;
                 polynom = 0;
                 promezh = 1;
             }
         }

         static void Comparison(double[] forFunction, double[] forCubis, double[] forLagranzh, double loop_step, double h)
         {
             Console.Write("x\t");
             Console.Write("Function\t");
             Console.Write("CubicInt\t");
             Console.Write("Lagranzh\t");
             Console.WriteLine();

             int k = 0;
             for (double x = 0.00; x <= loop_step; x += h, k++)
             {
                 Console.WriteLine("{0, 3:F2}\t{1, 7:F6}\t{2, 7:F6}\t{3, 7:F6}", x, forFunction[k], forCubis[k], forLagranzh[k]);
             }
         }

         static double Derivative1(double x, double h, int i, double[] forLagranzh, int size_steps)
        {

            if (i == 0) { return (forLagranzh[1] - forLagranzh[0]) / h; }
            else if (i == size_steps) { return (forLagranzh[size_steps] - forLagranzh[size_steps - 1]) / h; }
            else if (i != 0 && i != size_steps) { return (forLagranzh[i + 1] - forLagranzh[i - 1]) / (2 * h); }
            else { return 0; }
            /* switch (i)
              {
                  case 0: return (forLagranzh[1] - forLagranzh[0]) / h;
                  case 150: return (forLagranzh[size_steps] - forLagranzh[size_steps-1]) / h;
                  default: return (forLagranzh[i + 1] - forLagranzh[i - 1]) / (2*h); 
              }*/
        }

        static double Derivative2(double x, double h, int i, double[] forLagranzh, int size_steps)
         {
             if (i == 0) { return ( (-1) * forLagranzh[i + 3] + 4 * forLagranzh[i + 2] - 5 * forLagranzh[i + 1] + 2 * forLagranzh[i]) / (h * h); }
            else if (i == size_steps) { return ((-1) * forLagranzh[i - 3] + 4 * forLagranzh[i - 2] - 5 * forLagranzh[i - 1] + 2 * forLagranzh[i]) / (h * h); }
            else if (i != 0 && i != size_steps) { return (forLagranzh[i + 1] - 2 * forLagranzh[i]  + forLagranzh[i-1]) / (h * h); }
            else { return 0; }
            /*switch (i)
             {
                 case 0: return ( (-1) * forLagranzh[i + 3] + 4 * forLagranzh[i + 2] - 5 * forLagranzh[i + 1] + 2 * forLagranzh[i]) / (h * h);                                                                                                
                 case 150: return ( (-1) * forLagranzh[i - 3] + 4 * forLagranzh[i - 2] - 5 * forLagranzh[i - 1] + 2 * forLagranzh[i]) / (h * h);
                 default: return (forLagranzh[i + 1] - 2 * forLagranzh[i]  + forLagranzh[i-1]) / (h * h);
             }*/
         }

        }

}