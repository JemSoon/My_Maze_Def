// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("WdrU2+tZ2tHZWdra22foITq+1osUVQvlFg1s5KBwDw5jlicTVaPOoWlaCuquOcIYzc21QRCGiewX9RtPbwBXN5C+UeJWjo4qVikPzvG2hWwgLUK4/LaN9S1SvxLOSdmH/Anl51/6LzhZDVSuNd2HfNNaU2QfGieIUZPXAErhoUkVwEvdD2N+E5hUJk17UXFujtSzoLZjTfzEwpafsJUAoWpfPh9xcUtY3ODbIqMopM1c4D/PrUmrIIcv02j6FjM+HLcwPzQ9PFjRq5deBPxee/KDtA5THFmbdPpyg5+AueGtCBZpjXXknT22qk5LhQOi61na+evW3dLxXZNdLNba2tre29iNyLTD30k64oPJcoMWU7oOvbisFqSSh5rcjXPXCNnY2tva");
        private static int[] order = new int[] { 1,7,2,8,9,13,12,11,10,10,12,12,12,13,14 };
        private static int key = 219;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
