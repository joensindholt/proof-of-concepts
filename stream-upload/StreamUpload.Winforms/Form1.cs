using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace StreamUpload.Winforms;

public partial class Form1 : Form
{
    private const string haarPath = "C:\\dev\\proof-of-concept\\stream-upload\\StreamUpload.Winforms\\Resources\\haarcascade_frontalface_alt_tree.xml";

    private VideoCapture? _capture;
    private CascadeClassifier? _cascadeClassifier;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        _capture = new VideoCapture(0);
        _cascadeClassifier = new CascadeClassifier(haarPath);

        Task.Run(() =>
        {
            while (true)
            {
                Start();
            }
        });
    }

    private void Start()
    {
        using var imageFrame = _capture!.RetrieveMat();

        using var gray = new Mat();

        Cv2.CvtColor(imageFrame, gray, ColorConversionCodes.BGR2GRAY);

        var rects = _cascadeClassifier!.DetectMultiScale(gray, 1.08, 2, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(20, 20));

        foreach (var rect in rects)
        {
            Cv2.Rectangle(imageFrame, rect, Scalar.Red);
        }

        imgCam.Image = imageFrame.ToBitmap();
    }
}