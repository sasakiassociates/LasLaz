using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
using laszip.net;

namespace LasLaz
{
	public struct PointCloud
	{
		public readonly List<Vector3> points;
		public readonly List<Color> colors;
		public readonly List<Byte> classifications;

		public PointCloud(List<Vector3> points, List<Color> colors, List<byte> classifications)
		{
			this.points = points;
			this.colors = colors;
			this.classifications = classifications;
		}
	}

	public class Handler
	{

		private const string v353  ="115F75F2BEEC550E9246B2944870E38B";
		private const string v28  ="2022-04-13-45A54-equator-point";
		private readonly string testPath = "D:\\Projects\\ViewTo\\viewto-projects\\inglewood\\" + $"{v353}.laz";

		public List<string> RunWithFile()
		{
			var filename = Path.GetFullPath(testPath);

			var info = new List<string>();
			var lazReader = new laszip_dll();

			var compressed = true;
			lazReader.laszip_open_reader(filename, ref compressed);

			info.Add("Points: " + lazReader.header.number_of_point_records.ToString("N0"));
			info.Add("Returns: " + lazReader.header.number_of_points_by_return.Length);
			info.Add("File source ID: " + lazReader.header.file_source_ID);
			info.Add("Created on: " + lazReader.header.file_creation_day + " day of " + lazReader.header.file_creation_year);
			info.Add("Created with: " + Encoding.Default.GetString(lazReader.header.generating_software));

			return info;
		}

		public PointCloud ReadFile()
		{
			var filename = Path.GetFullPath(testPath);

			var lazReader = new laszip_dll();

			var compressed = true;
			lazReader.laszip_open_reader(filename, ref compressed);
			var numberOfPoints = lazReader.header.number_of_point_records;

			var vlrs = lazReader.header.vlrs;

			var colors = new List<Color>();
			var points = new List<Vector3>();
			var classifications = new List<Byte>();

			var coordArray = new double[3];

			for (int pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
			{
				// Read the point
				lazReader.laszip_read_point();

				// Get precision coordinates
				lazReader.laszip_get_coordinates(coordArray);
				points.Add(new Vector3((float)coordArray[0], (float)coordArray[1], (float)coordArray[2]));

				// Get classification value for sorting into branches
				var classification = lazReader.point.classification;
				classifications.Add(classification);

				colors.Add(Color.FromArgb(lazReader.point.rgb[0], lazReader.point.rgb[1], lazReader.point.rgb[2]));
			}
			lazReader.laszip_close_reader();

			return new PointCloud(points, colors, classifications);
		}

	}
}