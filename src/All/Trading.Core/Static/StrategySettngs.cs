using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Trading.Core.Static
{
    public static class StrategySettngs
    {
        public static Dictionary<int, (string name, string path, string secret)> BuyOrders =
            new Dictionary<int, (string name, string path, string secret)>
            {
                { 0, ("Open Long", "/tqCfWTfQ0pZH605VqFUK", "5s96qxxfynn") },
                { 1, ("0.6%-1 Long", "/0cnCqhh9ZRyLm2Kqq1UK", "atdyezqvvnu") },
                { 2, ("0.6%-2 Long", "/avkuwGYTKJu6j-xVqFUK", "loda8m7wrh") },
                { 3, ("0.75%-3 Long", "/OeLuGEBiygVl0eBVqFUK", "yb1x11b40wg") },
                { 4, ("0.75%-4 Long", "/M86VEsxX4epTY-RVqFUK", "9y3q5idfsxw") },
                { 5, ("1.25%-5 Long", "/xRGtVzCxI9pz2OVVqFUK", "d4x8uy4d42d") },
                { 6, ("1.25%-6 Long", "/hHwwYChvwlqZ6fpVqFUK", "yiskpl77vwj") },
                { 7, ("1.25%-7 Long", "/3Pw989weALtrGPtVqFUK", "nn6l8dwobnd") },
                { 8, ("2%-8 Long", "/4zmmB5cJ63r7l_9VqFUK", "l8i7z716ij") },
                { 9, ("2%-9 Long", "/o_9hiB-2fot8vvNVqFUK", "crpj8fefhzv") },
                { 10, ("3%-10 Long", "/VuRULyU8Ja6hGPZVqFUK", "gfpiaxr7rxg") },
                { 11, ("5%-11 Long", "/4psannwSWCpAEMJVqFUK", "m0zx28sxih8") },
                { 12, ("7%-12 Long", "/G1uCJenzkzJTOsNVqFUK", "1u36qsme63v") },
                { 1000, ("Close Long", "/U3ICqtlciJCeaPZRqFUK", "kct1uy7z0hh") }
            };
        public static Dictionary<int, (string name, string path, string secret)> SellOrders =
            new Dictionary<int, (string name, string path, string secret)>
            {
                { 0, ("Open Short", "/mrcyd3_zRDoNObZXqFUK", "utkcx2pzsr") },
                { 1, ("0.6%-1 Short", "/4YRslUPN8UHGXNlVqFUK", "dlzffcqeiqr") },
                { 2, ("0.6%-2 Short", "/NikfYq8UnE8zo95VqFUK", "k5q1f4fzl2m") },
                { 3, ("0.75%-3 Short", "/xp0Hdqwfi7qWnd9VqFUK", "f9zxqgxgrxv") },
                { 4, ("0.75%-4 Short", "/3pKejGysGS0EttxVqFUK", "szw7d3y9ahp") },
                { 5, ("1.25%-5 Short", "/JEpUiDWE_Vz7it1VqFUK", "q70iauwtve") },
                { 6, ("1.25%-6 Short", "/eyMaIFrx7aS57tNVqFUK", "b4ukt63r7uf") },
                { 7, ("1.25%-7 Short", "/Qf646_IxcWv0GtBVqFUK", "f63a2xdo91h") },
                { 8, ("2%-8 Short", "/Bu1bgo8GMP_3gtFVqFUK", "qargr2utpkp") },
                { 9, ("2%-9 Short", "/o_9hiB-2fot8vvNVqFUK", "k00h8zbvr5i") },
                { 10, ("3%-10 Short", "/ewc5YzWTDBo5iddVqFUK", "wu21uctn6gs") },
                { 11, ("5%-11 Short", "/LS4ZckWPTgzyl9RVqFUK", "29diaq0pcnd") },
                { 12, ("7%-12 Short", "/TBIHhu0DFZVxfdVVqFUK", "34hqylfs9an") },
                { 1000, ("Close Short", "/47flEWTscYho9fRRqFUK", "ix42eh3qu6f") }
            };
    }
}
