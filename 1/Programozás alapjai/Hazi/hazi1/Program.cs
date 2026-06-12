using System;
namespace hazi1
{
    internal class Program
    {
        static void Main(string[] args)
        {
        //Struktogram
        //https://progalap.elte.hu/stuki/?data=H4sIAAAAAAAACq1UXW%2FTMBT9L7cSD8NCJuln0F462KiqMrZqFWzqg2PfNBapXRy3Y1T578hp47qsewAtL746sc%2B5595rb0EKSIC%2B6%2FZoHL3v97txN%2B5F7S4QyPUGzUhAotZFQaDEArlF4RBYFpubxYxrIKC0wBKSLczE14tFcWVdXLN6gMCKGVT2QJZLIVBBkrGiRAL2aYWQQIk%2F16g4AgGey0IYVCNRQvIAt3h3%2BfFmMoZ5RSAeM%2Fxy0Ym9kAdCocOZ%2F5D7NtCL35%2BuaznP08gFxIFc4Pa03GMui1pLKyGt1AoSUPqcAoFUi6fpPpvRkaOKgL2jU9aLLr2%2BB47tNjn%2Fu13fTWd3eLXc3MvvEy%2FngSO5znia5ve3%2BKKc0nrl0vcbG77g5Ouk7xOsu9Xw%2BG4diAO5YGROy8nsr1bJM%2FmmsB%2FOFRCwZo1DwxTPj7oWNKYmO7HFm6%2FIoexNqsGtClINWF%2BojGUWl6jc6Fn8ZXdzpfRbeSahqgiU1qy51QvDlq5gW1Bs6Q5OmHRuNsxIlhbuFgMQMFo%2Fm%2Bgy14%2Bzw7a9vkM%2FIxNo9lA1dzUrtKkfhF0uD9DKMkqpm%2FOWWzNX2VaWcY%2BltOOjrI56NKasjjiyrN%2BD%2Bc7akPEfC6PXSuyJ0yiN3Da3NsTIGwx7nDVRVkeizSlGjs69W69IF9T4Gav79p4557UrznmDZVGDYSb4DhsMdn8p7fNBx0WI2BbdsCJV9QfE2AxBvgUAAA%3D%3D
        //Specifikacio
        //https://progalap.elte.hu/specifikacio/?data=H4sIAAAAAAAACo2NsQrCMBiEX%2BXnJpVYGgWHH7MITqKLdNE6hJpCQP9KE6fSvc%2Fpk0gFFSe7HHfHfVyDcHOFL31ho68EjJVjkkfX7fI6l41nkuqd1iXTtDdZ2ddkaH%2FItiNvdJKIIj%2FxL1kaGUMhuhAD%2BNjgbKMFg4Qp7XGSiimFgtirAwMKtQv3SwTrVv3sZ19gPpD4AHrQXi%2F%2BP5zaJ58vmhQoAQAA

            int n, no, i;

            Console.Write("kérek egy számot: ");
            n = int.Parse(Console.ReadLine());

            no = 0;
            for (i = 1; i <= n; i++)
            {
                if (i * i <= n)
                {
                    no = no + i * i;
                }
            }
            Console.WriteLine("Az 1 és {0} koze eso negyzetszamok osszege: {1}", n, no);
        }
    }
}
