namespace WorkLife.Model.Contract
{
    public interface IPerson
    {
        string EmployeeId { get; } 
        
        string Name { get; }

        void TrackTime(IndustryTime timeToTrack, DateOnly date);

        IndustryTime GetWorkingTimeByDate(DateOnly date);

        (IndustryTime, IndustryTime) GetDailyTotalAndTarget(DateOnly date);

        (IndustryTime, IndustryTime) GetMonthlyTotalAndTarget(DateOnly date);
    }
}
