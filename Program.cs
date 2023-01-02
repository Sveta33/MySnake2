namespace MySnake2
{
    public class Program
    {
        //Preparing a game field with a starting point
        //Подготовка игрового поля с отправной точкой
        class Game
        {
            static readonly int x = 80;
            static readonly int y = 26;

            static Walls walls;
            static Snake snake;
            //static FoodFactory foodFactory;
            static Timer time;

            static void Main()
            {
                Console.SetWindowSize(x + 1, y + 1);
                Console.SetBufferSize(x + 1, y + 1);
                Console.CursorVisible = false;

                walls = new Walls(x, y, '#');
                snake = new Snake(x / 2, y / 2, 3);
                
                time = new Timer(Loop, null, 0, 200);

                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        snake.Rotation(key.Key);
                    }
                }
            }// Main()

            static void Loop(object obj)
            {
                if (walls.IsHit(snake.GetHead()) || snake.IsHit(snake.GetHead()))
                {
                    time.Change(0, Timeout.Infinite);
                }
                
                else
                {
                    snake.Move();
                }
            }// Loop()
        }// class Game
        //display graphics
        //вывода на экран графики
        struct Point
        {
            public int x { get; set; }
            public int y { get; set; }
            public char ch { get; set; }
            /*The => operator is used as the definition of anonymous expressions,
              and the replacement operator return*/
            public static implicit operator Point((int, int, char) value) =>
                  new Point { x = value.Item1, y = value.Item2, ch = value.Item3 };

            public static bool operator ==(Point a, Point b) =>
                    (a.x == b.x && a.y == b.y) ? true : false;
            public static bool operator !=(Point a, Point b) =>
                    (a.x != b.x || a.y != b.y) ? true : false;
            //вывод на экран точки и ее стирания
            //displaying a point and erasing it
            public void Draw()
            {
                DrawPoint(ch);
            }
            public void Clear()
            {
                DrawPoint(' ');
            }

            private void DrawPoint(char _ch)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(_ch);
            }
        }
        //wall class, playing field boundaries
        //класс стен, границы игрового поля
        class Walls
        {
            private char ch;
            private List<Point> wall = new List<Point>();

            public Walls(int x, int y, char ch)
            {
                this.ch = ch;

                DrawHorizontal(x, 0);
                DrawHorizontal(x, y);
                DrawVertical(0, y);
                DrawVertical(x, y);
            }

            private void DrawHorizontal(int x, int y)
            {
                for (int i = 0; i < x; i++)
                {
                    Point p = (i, y, ch);
                    p.Draw();
                    wall.Add(p);
                }
            }

            private void DrawVertical(int x, int y)
            {
                for (int i = 0; i < y; i++)
                {
                    Point p = (x, i, ch);
                    p.Draw();
                    wall.Add(p);
                }
            }

            public bool IsHit(Point p)
            {
                foreach (var w in wall)
                {
                    if (p == w)
                    {
                        return true;
                    }
                }
                return false;
            }
        }// class Walls

        enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        class Snake
        {
            private List<Point> snake;

            private Direction direction;
            private int step = 1;
            private Point tail;
            private Point head;

            bool rotate = true;

            public Snake(int x, int y, int length)
            {
                direction = Direction.RIGHT;

                snake = new List<Point>();
                for (int i = x - length; i < x; i++)
                {
                    Point p = (i, y, '*');
                    snake.Add(p);

                    p.Draw();
                }
            }

            public Point GetHead() => snake.Last();

            public void Move()
            {
                head = GetNextPoint();
                snake.Add(head);

                tail = snake.First();
                snake.Remove(tail);

                head.Draw();

                rotate = true;
            }
            public Point GetNextPoint()
            {
                Point p = GetHead();

                switch (direction)
                {
                    case Direction.LEFT:
                        p.x -= step;
                        break;
                    case Direction.RIGHT:
                        p.x += step;
                        break;
                    case Direction.UP:
                        p.y -= step;
                        break;
                    case Direction.DOWN:
                        p.y += step;
                        break;
                }
                return p;
            }

            public void Rotation(ConsoleKey key)
            {
                if (rotate)
                {
                    switch (direction)
                    {
                        case Direction.LEFT:
                        case Direction.RIGHT:
                            if (key == ConsoleKey.DownArrow)
                                direction = Direction.DOWN;
                            else if (key == ConsoleKey.UpArrow)
                                direction = Direction.UP;
                            break;
                        case Direction.UP:
                        case Direction.DOWN:
                            if (key == ConsoleKey.LeftArrow)
                                direction = Direction.LEFT;
                            else if (key == ConsoleKey.RightArrow)
                                direction = Direction.RIGHT;
                            break;
                    }
                    rotate = false;
                }

            }

            public bool IsHit(Point p)
            {
                for (int i = snake.Count - 2; i > 0; i--)
                {
                    if (snake[i] == p)
                    {
                        return true;
                    }
                }
                return false;
            }
        }//class Snake

    }

    }


