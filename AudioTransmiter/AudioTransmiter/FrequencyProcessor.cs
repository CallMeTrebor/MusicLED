using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTransmiter
{
	internal class FrequencyProcessor
	{
		/// <summary>
		/// Normalizeses a whole array of frequencies between fmin and fmax to values in [0, 1]
		/// </summary>
		/// <param name="fmin">Minimum value, corresponding to 0</param>
		/// <param name="fmax">Maximum value, corresponding to 1</param>
		/// <param name="frequencies">The input frequencies</param>
		/// <returns>Normalized freqency array</returns>
		public static float[] NormalizedFrequencyArray(int fmin, int fmax, float[] frequencies)
		{
			for (int i = 0; i < frequencies.Length; i++)
			{
				float f = frequencies[i];
				frequencies[i] = (f - fmin) / (fmax - fmin);
			}

			return frequencies;
		}

		//Be sure to use normalized frequencies for the functions below

		/// <summary>
		/// Returns the R component of the RGB code generated according to the input normalized frequencies
		/// </summary>
		/// <param name="frequencies">An arrray of noramlized  frequencies [0, 1]</param>
		/// <returns>R component based on the input frequencies</returns>
		public static int R(float[] frequencies)
		{
			int r = 0;
			for (int i = 0; i < frequencies.Length; i++)
			{
				r += (int)(256 * (1 - frequencies[i]));

			}
			return r / frequencies.Length;
		}

		/// <summary>
		/// Returns the G component of the RGB code generated according to the input normalized frequencies
		/// </summary>
		/// <param name="frequencies">An arrray of noramlized  frequencies [0, 1]</param>
		/// <returns>G component based on the input frequencies</returns>
		public static int G(float[] frequencies)
		{
			int g = 0;
			for (int i = 0; i < frequencies.Length; i++)
			{
				g += (int)(256 * (1 - Math.Abs(2 * frequencies[i] - 1)));
			}
			return g / frequencies.Length;
		}

		/// <summary>
		/// Returns the B component of the RGB code generated according to the input normalized frequencies
		/// </summary>
		/// <param name="frequencies">An arrray of noramlized  frequencies [0, 1]</param>
		/// <returns>B component based on the input frequencies</returns>
		public static int B(float[] frequencies)
		{
			int b = 0;
			for (int i = 0; i < frequencies.Length; i++)
			{
				b += (int)(256 * frequencies[i]);
			}
			return b / frequencies.Length;
		}
	}
}
