// namespace AdventOfCode2023;
//
// public class CombinationGenerator<T>
// {
//     private readonly IEnumerable<T> _itemPool;
//
//     public CombinationGenerator(IEnumerable<T> itemPool)
//     {
//         _itemPool = itemPool;
//     }
//
//     public IList<IList<T>> GetCombinations()
//     {
//         return null;
//     }
//
//     private IList<IList<T>> GetCombinations(int numItems)
//     {
//     }
//
//     static void CombinationRepetitionUtil(int[] chosen, int[] arr,
//         int index, int r, int start, int end)
//     {
//         // Since index has become r, current combination is 
//         // ready to be printed, print 
//         if (index == r)
//         {
//             for (int i = 0; i < r; i++)
//             {
//                 Console.Write(arr[chosen[i]] + " ");
//             }
//
//             Console.WriteLine();
//             return;
//         }
//
//         // One by one choose all elements (without considering 
//         // the fact whether element is already chosen or not) 
//         // and recur 
//         for (int i = start; i <= end; i++)
//         {
//             chosen[index] = i;
//             CombinationRepetitionUtil(chosen, arr, index + 1,
//                 r, i, end);
//         }
//
//         return;
//     }
//
// // The main function that prints all combinations of size r 
// // in arr[] of size n with repetitions. This function mainly 
// // uses CombinationRepetitionUtil() 
//     static void CombinationRepetition(int[] arr, int n, int r)
//     {
//         // Allocate memory 
//         int[] chosen = new int[r + 1];
//
//         // Call the recursive function 
//         CombinationRepetitionUtil(chosen, arr, 0, r, 0, n - 1);
//     }
// }