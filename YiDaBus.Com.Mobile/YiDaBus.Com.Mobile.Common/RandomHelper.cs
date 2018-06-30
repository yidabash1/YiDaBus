using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiDaBus.Com.Mobile.Common
{
    public static class RandomHelper
    {
        public static string GenetatorNumbers(int number = 4)
        {
            int[] face = new int[number];
            Random ra = new Random();
            for (int i = 0; i < face.Length; i++)
            {
                int count = 0;
                face[i] = ra.Next(0, 2);
                for (int j = 0; j < i; j++)
                {
                    if (face[i] == face[j])
                    {
                        count++;
                        if (count == 2)
                        {
                            i--;
                            break;
                        }
                    }
                }
            }
            string r = string.Empty;
            foreach (var item in face)
            {
                r += item;
            }
            return r;
        }
    }
}
