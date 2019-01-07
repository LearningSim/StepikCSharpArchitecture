using System;
using System.Windows.Forms;
using MyPhotoshop.Data;
using System.Drawing;

namespace MyPhotoshop {
    class MainClass {
        [STAThread]
        public static void Main(string[] args) {
            var window = new MainWindow();
            window.AddFilter(new PixelFilter<LighteningParameters>(
                "����������/����������",
                (pixel, parameters) => pixel * parameters.Coefficient
            ));
            window.AddFilter(new PixelFilter<EmptyParameters>(
                "������� ������",
                (pixel, parameters) => {
                    double gray = pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114;
                    return new Pixel(gray, gray, gray);
                }
            ));
            window.AddFilter(new TransformFilter(
                "�������� �� �����������",
                size => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y)
	        ));
            window.AddFilter(new TransformFilter<RotationParameters>(
                "���������", new RotationAlgorithm()
            ));
            Application.Run(window);
        }
    }
}
