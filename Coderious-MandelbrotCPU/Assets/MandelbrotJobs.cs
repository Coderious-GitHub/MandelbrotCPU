using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

public class MandelbrotJobs : MonoBehaviour
{
	double height, width;
	double rStart, iStart;
	int maxIterations;
	int zoom;

	Texture2D display;
	public Image image;

	public TextMeshProUGUI timePerFrame;

	Color32[] colors;

	[BurstCompile]
	struct Mandelbrot : IJobParallelFor
    {
		public NativeArray<double> x;
		public NativeArray<double> y;
		public int maxIterations;
		public NativeArray<int> result;

		public void Execute(int r)
        {
			result[r] = 0;
			Complex z = new Complex(0, 0);

			//Cardiod-Bulb Checking
			double q = (x[r] - 0.25) * (x[r] - 0.25) + y[r] * y[r];
			if (q * (q + (x[r] - 0.25)) < 0.25 * (y[r] * y[r]))
			{
				result[r] = maxIterations;
				return;
			}

			for (int i = 0; i != maxIterations; i++)
			{
				z = z * z + new Complex(x[r], y[r]);

				if (Complex.Abs(z) > 2)
				{
					return;
				}
				else
				{
					result[r]++;
				}
			}
		}
    }

	[BurstCompile]
	struct SetColor : IJobParallelFor
    {
		public NativeArray<int> value;
		public int maxIterations;
		public NativeArray<Color32> colors;

		public void Execute(int r)
        {
			Color32 color = new Color32(0, 0, 0, 255);

			if (value[r] != maxIterations)
			{
				int colorNr = value[r] % 16;

				switch (colorNr)
				{
					case 0:
						{
							color[0] = 66;
							color[1] = 30;
							color[2] = 15;

							break;
						}
					case 1:
						{
							color[0] = 25;
							color[1] = 7;
							color[2] = 26;
							break;
						}
					case 2:
						{
							color[0] = 9;
							color[1] = 1;
							color[2] = 47;
							break;
						}

					case 3:
						{
							color[0] = 4;
							color[1] = 4;
							color[2] = 73;
							break;
						}
					case 4:
						{
							color[0] = 0;
							color[1] = 7;
							color[2] = 100;
							break;
						}
					case 5:
						{
							color[0] = 12;
							color[1] = 44;
							color[2] = 138;
							break;
						}
					case 6:
						{
							color[0] = 24;
							color[1] = 82;
							color[2] = 177;
							break;
						}
					case 7:
						{
							color[0] = 57;
							color[1] = 125;
							color[2] = 209;
							break;
						}
					case 8:
						{
							color[0] = 134;
							color[1] = 181;
							color[2] = 229;
							break;
						}
					case 9:
						{
							color[0] = 211;
							color[1] = 236;
							color[2] = 248;
							break;
						}
					case 10:
						{
							color[0] = 241;
							color[1] = 233;
							color[2] = 191;
							break;
						}
					case 11:
						{
							color[0] = 248;
							color[1] = 201;
							color[2] = 95;
							break;
						}
					case 12:
						{
							color[0] = 255;
							color[1] = 170;
							color[2] = 0;
							break;
						}
					case 13:
						{
							color[0] = 204;
							color[1] = 128;
							color[2] = 0;
							break;
						}
					case 14:
						{
							color[0] = 153;
							color[1] = 87;
							color[2] = 0;
							break;
						}
					case 15:
						{
							color[0] = 106;
							color[1] = 52;
							color[2] = 3;
							break;
						}
				}
			}
			colors[r] = color;
		}
    }


	// Start is called before the first frame update
	void Start()
	{
		width = 4.5;
		height = width * Screen.height / Screen.width;
		rStart = -2.0;
		iStart = -1.25;
		zoom = 10;
		maxIterations = 100;

		display = new Texture2D(Screen.width, Screen.height);
		colors = new Color32[Screen.width * Screen.height];
		RunMandelbrot();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			rStart = rStart + (Input.mousePosition.x - (Screen.width / 2.0)) / Screen.width * width;
			iStart = iStart + (Input.mousePosition.y - (Screen.height / 2.0)) / Screen.height * height;
			RunMandelbrot();
		}

		if (Input.mouseScrollDelta.y != 0)
		{
			double wFactor = width * (double)Input.mouseScrollDelta.y / zoom;
			double hFactor = height * (double)Input.mouseScrollDelta.y / zoom;
			width -= wFactor;
			height -= hFactor;
			rStart += wFactor / 2.0;
			iStart += hFactor / 2.0;

			if (Input.mouseScrollDelta.y > 0)
            {
				maxIterations += 3;
			}
            else
            {
				maxIterations -= 3;
            }

			RunMandelbrot();
		}
	}

	void RunMandelbrot()
	{
		float startTime = Time.realtimeSinceStartup;

		NativeArray<int> result = new NativeArray<int>(display.width * display.height, Allocator.TempJob);
		NativeArray<Color32> color = new NativeArray<Color32>(display.width * display.height, Allocator.TempJob);
		NativeArray<double> xList = new NativeArray<double>(display.width * display.height, Allocator.TempJob);
		NativeArray<double> yList = new NativeArray<double>(display.width * display.height, Allocator.TempJob);

		for (int y = 0; y != display.height; y++)
		{
			for (int x = 0; x != display.width; x++)
			{
				xList[x + y * display.width] = rStart + width * (double)x / display.width;
				yList[x + y * display.width] = iStart + height * (double)y / display.height;
			}
		}

		Mandelbrot mandelbrot = new Mandelbrot()
		{
			x = xList,
			y = yList,
			maxIterations = maxIterations,
			result = result
		};

		JobHandle handle = mandelbrot.Schedule(result.Length, 16);
		handle.Complete();

		SetColor setColor = new SetColor()
		{
			value = result,
			maxIterations = maxIterations,
			colors = color
		};

		JobHandle handle2 = setColor.Schedule(result.Length, 16, handle);
		handle2.Complete();

		for (int y = 0; y != display.height; y++)
		{
			for (int x = 0; x != display.width; x++)
			{
				colors[x + y * display.width] = color[x + y * display.width];
			}
		}

		display.SetPixels32(colors);

		display.Apply();
		image.sprite = Sprite.Create(display, new Rect(0, 0, display.width, display.height),
			new UnityEngine.Vector2(0.5f, 0.5f));

		float endTime = Time.realtimeSinceStartup;
		timePerFrame.text = (endTime - startTime).ToString();

		result.Dispose();
		color.Dispose();
		xList.Dispose();
		yList.Dispose();
	}
}
