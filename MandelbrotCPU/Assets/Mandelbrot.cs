using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mandelbrot : MonoBehaviour
{
	double height, width;
	double rStart, iStart;
	int maxIterations;
	int zoom;

	Texture2D display;
	public Image image;

	public TextMeshProUGUI timePerFrame;


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

			RunMandelbrot();
		}
	}

	void RunMandelbrot()
	{
		float startTime = Time.realtimeSinceStartup;

		for (int x = 0; x != display.width; x++)
		{
			for (int y = 0; y != display.height; y++)
			{
				display.SetPixel(x, y, SetColor(MandelbrotFunction(rStart + width * (double)x / display.width,
					iStart + height * (double)y / display.height)));
			}
		}

		display.Apply();
		image.sprite = Sprite.Create(display, new Rect(0, 0, display.width, display.height),
			new UnityEngine.Vector2(0.5f, 0.5f));

		float endTime = Time.realtimeSinceStartup;

		timePerFrame.text = (endTime - startTime).ToString();
	}

	int MandelbrotFunction(double x, double y)
	{
		int iteration = 0;

		Complex z = new Complex(0, 0);

		for (int i = 0; i != maxIterations; i++)
		{
			z = z * z + new Complex(x, y);

			if (Complex.Abs(z) > 2)
			{
				return i;
			}
			else
			{
				iteration++;
			}
		}

		return iteration;
	}

	Color SetColor(int value)
	{
		UnityEngine.Vector4 CalcColor = new UnityEngine.Vector4(0, 0, 0, 1f);

		if (value != maxIterations)
		{
			int colorNr = value % 16;

			switch (colorNr)
			{
				case 0:
					{
						CalcColor[0] = 66.0f / 255.0f;
						CalcColor[1] = 30.0f / 255.0f;
						CalcColor[2] = 15.0f / 255.0f;

						break;
					}
				case 1:
					{
						CalcColor[0] = 25.0f / 255.0f;
						CalcColor[1] = 7.0f / 255.0f;
						CalcColor[2] = 26.0f / 255.0f;
						break;
					}
				case 2:
					{
						CalcColor[0] = 9.0f / 255.0f;
						CalcColor[1] = 1.0f / 255.0f;
						CalcColor[2] = 47.0f / 255.0f;
						break;
					}

				case 3:
					{
						CalcColor[0] = 4.0f / 255.0f;
						CalcColor[1] = 4.0f / 255.0f;
						CalcColor[2] = 73.0f / 255.0f;
						break;
					}
				case 4:
					{
						CalcColor[0] = 0.0f / 255.0f;
						CalcColor[1] = 7.0f / 255.0f;
						CalcColor[2] = 100.0f / 255.0f;
						break;
					}
				case 5:
					{
						CalcColor[0] = 12.0f / 255.0f;
						CalcColor[1] = 44.0f / 255.0f;
						CalcColor[2] = 138.0f / 255.0f;
						break;
					}
				case 6:
					{
						CalcColor[0] = 24.0f / 255.0f;
						CalcColor[1] = 82.0f / 255.0f;
						CalcColor[2] = 177.0f / 255.0f;
						break;
					}
				case 7:
					{
						CalcColor[0] = 57.0f / 255.0f;
						CalcColor[1] = 125.0f / 255.0f;
						CalcColor[2] = 209.0f / 255.0f;
						break;
					}
				case 8:
					{
						CalcColor[0] = 134.0f / 255.0f;
						CalcColor[1] = 181.0f / 255.0f;
						CalcColor[2] = 229.0f / 255.0f;
						break;
					}
				case 9:
					{
						CalcColor[0] = 211.0f / 255.0f;
						CalcColor[1] = 236.0f / 255.0f;
						CalcColor[2] = 248.0f / 255.0f;
						break;
					}
				case 10:
					{
						CalcColor[0] = 241.0f / 255.0f;
						CalcColor[1] = 233.0f / 255.0f;
						CalcColor[2] = 191.0f / 255.0f;
						break;
					}
				case 11:
					{
						CalcColor[0] = 248.0f / 255.0f;
						CalcColor[1] = 201.0f / 255.0f;
						CalcColor[2] = 95.0f / 255.0f;
						break;
					}
				case 12:
					{
						CalcColor[0] = 255.0f / 255.0f;
						CalcColor[1] = 170.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 13:
					{
						CalcColor[0] = 204.0f / 255.0f;
						CalcColor[1] = 128.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 14:
					{
						CalcColor[0] = 153.0f / 255.0f;
						CalcColor[1] = 87.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 15:
					{
						CalcColor[0] = 106.0f / 255.0f;
						CalcColor[1] = 52.0f / 255.0f;
						CalcColor[2] = 3.0f / 255.0f;
						break;
					}
			}
		}
		return CalcColor;
	}

}
