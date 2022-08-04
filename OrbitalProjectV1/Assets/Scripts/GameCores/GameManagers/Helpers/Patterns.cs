using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDataMgt;

/// <summary>
/// This is the pattern class.
/// It generates the different patterns according to input.
/// </summary>
namespace GameManagement.Helpers
{
    public class Patterns
    {

        Vector2 startPos;
        Vector2 endPos;
        private int n;

        private Patterns(Vector2 _startPos, Vector2 _endPos)
        {
            startPos = _startPos;
            endPos = _endPos;
            n = Mathf.Min(Mathf.Abs((int)(startPos.x - endPos.x)), Mathf.Abs((int)(startPos.y - endPos.y)));
        }

        public static Patterns of(Vector2 startpos, Vector2 endpos)
        {
            return new Patterns(startpos, endpos);
        }

        /** Diagonal Pattern.
         * returns a list of vectors in the diagonal matrix.
         */
        public List<Vector2> Diagonal()
        {
            int i = (int)startPos.x;
            int j = (int)startPos.y;
            List<Vector2> points = new List<Vector2>();
            int k = n;
            while (k-- > 0)
            {
                points.Add(new Vector2(i + k, j + k));
            }

            return points;
        }

        /** Cross Pattern.
         * returns a list of vectors in the cross matrix.
         */
        public List<Vector2> Cross()
        {
            int i = (int)startPos.x;
            int j = (int)startPos.y;
            HashSet<Vector2> points = new HashSet<Vector2>();
            //while (i < n + (int) startPos.x)
            //{
            //    points.Add(new Vector2(i, i));
            //    points.Add(new Vector2(i, 2 * startPos.x - i + n));
            //    i++;
            //}
            int k = n;
            while (k-- > 0)
            {
                points.Add(new Vector2(i + k, j + k));
                points.Add(new Vector2(i + k, j + n - k));
            }

            return new List<Vector2>(points);
        }

        /** Pattern1.
         * returns a list of vectors in the pattern1 matrix.
         */
        public List<Vector2> Pattern1()
        {
            int k = (int)startPos.x;
            int g = (int)startPos.y;
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < n; i++)
            {

                // Loop denoting columns
                for (int j = 0; j < n; j++)
                {
                    if (i == 0 || j == 0 || i == j
                    || i == n - 1 || j == n - 1
                    || i + j == n - 1)
                    {
                        points.Add(new Vector2(k + i, g + j));
                    }
                }
            }
            return points;
        }

        /** Pattern2.
         * returns a list of vectors in the pattern2 matrix.
         */
        public List<Vector2> Pattern2()
        {
            List<Vector2> points = new List<Vector2>();
            int k = (int)startPos.x;
            int g = (int)startPos.y;
            int c1 = (n - 1) / 2;

            // For printing lower portion
            int c2 = 3 * n / 2 - 1;

            // Loop denoting rows
            for (int i = 0; i < n; i++)
            {

                // Loop denoting columns
                for (int j = 0; j < n; j++)
                {

                    // Checking conditions for printing
                    // pattern
                    if (i + j == c1 || i - j == c1
                        || j - i == c1 || i + j == c2 ||
                        i == c1 || j == c1)
                    {
                        points.Add(new Vector2(k + i, g + j));
                    }

                }
            }

            return points;
        }

        /** Pattern3.
         * returns a list of vectors in the pattern3 matrix.
         */
        public List<Vector2> Pattern3()
        {
            List<Vector2> points = new List<Vector2>();
            int i = (int)startPos.x;
            int j = (int)startPos.y;
            for (int k = 0; k < n; k++)
            {
                for (int l = 0; l < (2 * n); l++)
                {
                    if (k + l <= n - 1 || (k + n) <= l)
                    {  // upper left triangle
                        points.Add(new Vector2(i + l, j + k));
                    }
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int l = 0; l < (2 * n); l++)
                {
                    if (k >= l || k >= (2 * n - 1) - l)
                    {
                        points.Add(new Vector2(i + l, j + k)); ;
                    }
                }
            }

            return points;
        }

        /** Box Pattern.
         * returns a list of vectors in the Box matrix.
         */
        public List<Vector2> Box()
        {
            List<Vector2> points = new List<Vector2>();
            for (int i = (int)startPos.x; i <= (int)endPos.x; i++)
            {
                for (int j = (int)startPos.y; j <= (int)endPos.y; j++)
                {
                    points.Add(new Vector2(i, j));
                }
            }
            return points;
        }

        /** Random Patterns.
         * returns a list of vectors of random patterns.
         */
        public List<Vector2> RandomPattern(EnemyData.PATTERN pattern)
        {
            List<Vector2> points;
            switch (pattern)
            {
                default:
                case EntityData.PATTERN.BOX_LINE:
                    points = this.Box();
                    break;
                case EntityData.PATTERN.CROSS:
                    points = this.Cross();
                    break;
                case EntityData.PATTERN.SIMPLE_DIAG:
                    points = this.Diagonal();
                    break;
                case EntityData.PATTERN.PATTERN1:
                    points = this.Pattern1();
                    break;
                case EntityData.PATTERN.PATTERN2:
                    points = this.Pattern2();
                    break;
                case EntityData.PATTERN.PATTERN3:
                    points = this.Pattern3();
                    break;
            }

            return points;
        }

    }
}