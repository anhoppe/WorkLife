
namespace WorkLife.Model.Contract
{
    /// <summary>
    /// DTO for the working week.
    /// </summary>
    public class TargetTimeWeek
    {
        public TargetTimeModel TargetTimeModel { get; set; } = TargetTimeModel.None;

        public DateOnly ValidFromDate { get; set; }

        /// <summary>
        /// Contains the working time of the week in minutes.
        /// Sun-Sat
        /// </summary>
        public int[] TargetWorkingTimeMin { get; set; } = [0, 0, 0, 0, 0, 0, 0];
    }
}
