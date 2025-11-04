using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork.Geometry
{
   
    interface IPrintable
    {
        void Print();
    }

   
    abstract class Shape : IPrintable
    {
        public abstract double CalculateArea();
        public abstract void SetVertices();
        public virtual void Print()
        {
            Console.WriteLine("Фігура без визначених вершин.");
        }

        public Shape()
        {
            Console.WriteLine("✅ Створено об'єкт Shape (базовий клас).");
        }

        ~Shape()
        {
            Console.WriteLine("🗑 Знищено об'єкт Shape.");
        }
    }

  
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}; {Y})";
    }

   
    class Triangle : Shape
    {
        private Point[] _points = new Point[3];

        public Triangle()
        {
            Console.WriteLine("✅ Створено об'єкт Triangle.");
        }

        ~Triangle()
        {
            Console.WriteLine("🗑 Викликано деструктор Triangle.");
        }

        public override void SetVertices()
        {
            Console.WriteLine("\nВведіть координати 3 вершин трикутника:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Вершина {i + 1} (x y): ");
                string[] parts = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                if (parts.Length != 2 ||
                    !double.TryParse(parts[0], out double x) ||
                    !double.TryParse(parts[1], out double y))
                {
                    Console.WriteLine("❌ Невірний формат! Повторіть ввід.");
                    i--;
                    continue;
                }
                _points[i] = new Point(x, y);
            }
        }

        public override void Print()
        {
            Console.WriteLine("\n🔺 Трикутник:");
            for (int i = 0; i < 3; i++)
                Console.WriteLine($"Вершина {i + 1}: {_points[i]}");
            Console.WriteLine($"Площа: {CalculateArea():F2}");
        }

        public override double CalculateArea()
        {
            double x1 = _points[0].X, y1 = _points[0].Y;
            double x2 = _points[1].X, y2 = _points[1].Y;
            double x3 = _points[2].X, y3 = _points[2].Y;

           
            return Math.Abs((x1 * (y2 - y3) +
                             x2 * (y3 - y1) +
                             x3 * (y1 - y2)) / 2.0);
        }
    }

   
    class ConvexQuadrilateral : Shape
    {
        private Point[] _points = new Point[4];

        public ConvexQuadrilateral()
        {
            Console.WriteLine("✅ Створено об'єкт ConvexQuadrilateral.");
        }

        ~ConvexQuadrilateral()
        {
            Console.WriteLine("🗑 Викликано деструктор ConvexQuadrilateral.");
        }

        public override void SetVertices()
        {
            Console.WriteLine("\nВведіть координати 4 вершин чотирикутника (у порядку обходу):");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Вершина {i + 1} (x y): ");
                string[] parts = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                if (parts.Length != 2 ||
                    !double.TryParse(parts[0], out double x) ||
                    !double.TryParse(parts[1], out double y))
                {
                    Console.WriteLine("❌ Невірний формат! Повторіть ввід.");
                    i--;
                    continue;
                }
                _points[i] = new Point(x, y);
            }

            if (!IsConvex())
            {
                Console.WriteLine("⚠️ Увага! Вказаний чотирикутник не є опуклим.");
            }
        }

        public override void Print()
        {
            Console.WriteLine("\n⬜ Опуклий чотирикутник:");
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"Вершина {i + 1}: {_points[i]}");
            Console.WriteLine($"Площа: {CalculateArea():F2}");
        }

        public override double CalculateArea()
        {
           
            double area = 0;
            for (int i = 0; i < 4; i++)
            {
                Point p1 = _points[i];
                Point p2 = _points[(i + 1) % 4];
                area += (p1.X * p2.Y - p2.X * p1.Y);
            }
            return Math.Abs(area) / 2.0;
        }

        private bool IsConvex()
        {
            bool? sign = null;
            for (int i = 0; i < 4; i++)
            {
                Point p0 = _points[i];
                Point p1 = _points[(i + 1) % 4];
                Point p2 = _points[(i + 2) % 4];

                double cross = (p1.X - p0.X) * (p2.Y - p1.Y) - (p1.Y - p0.Y) * (p2.X - p1.X);
                bool currentSign = cross > 0;

                if (sign == null)
                    sign = currentSign;
                else if (sign != currentSign)
                    return false;
            }
            return true;
        }
    }

  
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота: Абстрактні класи, інтерфейси, фігури ===\n");

            Console.WriteLine("Оберіть фігуру:");
            Console.WriteLine("1 - Трикутник");
            Console.WriteLine("2 - Опуклий чотирикутник");
            Console.Write("Ваш вибір: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.Write("❌ Невірний вибір! Введіть 1 або 2: ");
            }

            Shape shape = (choice == 1) ? new Triangle() : new ConvexQuadrilateral();
            shape.SetVertices();
            shape.Print();

            Console.WriteLine("\n=== Демонстрація інтерфейсу IPrintable ===");
            IPrintable printable = shape;
            printable.Print();

            Console.WriteLine("\n✅ Програму виконано успішно.");
        }
    }
}
