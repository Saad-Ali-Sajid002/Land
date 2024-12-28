using OpenCvSharp;

namespace Land.Services
{
    public class ImageAnalysisService
    {
        public bool IsDistressedProperty(string imagePath)
        {
            using var image = Cv2.ImRead(imagePath);

            // Example: Simple threshold analysis for detecting overgrown grass
            Mat hsv = new Mat();
            Cv2.CvtColor(image, hsv, ColorConversionCodes.BGR2HSV);

            var lowerGreen = new Scalar(25, 40, 40);
            var upperGreen = new Scalar(85, 255, 255);

            Mat mask = new Mat();
            Cv2.InRange(hsv, lowerGreen, upperGreen, mask);

            // Check if green area exceeds a threshold
            var greenArea = Cv2.CountNonZero(mask);
            return greenArea > 5000; // Arbitrary threshold
        }
    }

}
