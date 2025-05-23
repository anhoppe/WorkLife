
namespace WorkLife.Model.Contract
{
    public enum TargetTimeModel
    {
        None,

        FullTimeMonToFri,
        FullTimeMonToSat,
        FullTimeMonToSun,
        HalftTime24MonToSat,
        HalftTime24MonToWed,
        HalftTime24ThuToSat,
        HalftTime18MonToSat,
        HalftTime18MonToWed,
        HalftTime18ThuToSat,
    }

    public static class TargetTimeModelExtensions
    {
        public static TargetTimeModel ToTargetTimeModel(this int targetTimeModelId) => targetTimeModelId switch
        {
            1 => TargetTimeModel.FullTimeMonToFri,
            2 => TargetTimeModel.FullTimeMonToSat,
            3 => TargetTimeModel.FullTimeMonToSun,
            4 => TargetTimeModel.HalftTime24MonToSat,
            5 => TargetTimeModel.HalftTime24MonToWed,
            6 => TargetTimeModel.HalftTime24ThuToSat,
            7 => TargetTimeModel.HalftTime18MonToSat,
            8 => TargetTimeModel.HalftTime18MonToWed,
            9 => TargetTimeModel.HalftTime18ThuToSat,
            _ => throw new ArgumentOutOfRangeException(nameof(targetTimeModelId), targetTimeModelId, null)
        };

        public static int ToTargetTimeModelId(this TargetTimeModel targetTimeModel) => targetTimeModel switch
        {
            TargetTimeModel.None => 0,
            TargetTimeModel.FullTimeMonToFri => 1,
            TargetTimeModel.FullTimeMonToSat => 2,
            TargetTimeModel.FullTimeMonToSun => 3,
            TargetTimeModel.HalftTime24MonToSat => 4,
            TargetTimeModel.HalftTime24MonToWed => 5,
            TargetTimeModel.HalftTime24ThuToSat => 6,
            TargetTimeModel.HalftTime18MonToSat => 7,
            TargetTimeModel.HalftTime18MonToWed => 8,
            TargetTimeModel.HalftTime18ThuToSat => 9,
            _ => throw new ArgumentOutOfRangeException(nameof(targetTimeModel), targetTimeModel, null)
        };
    }
}
