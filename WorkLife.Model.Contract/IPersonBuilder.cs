namespace WorkLife.Model.Contract
{
    public interface IPersonBuilder
    {
        IPerson Build();

        IPersonBuilder WithName(string name);

        IPersonBuilder WithEmployeeId(string employeeId);

        IPersonBuilder WithId(int id);

        IPersonBuilder WithTargetTimeModel(DateOnly startDate, TargetTimeModel targetTimeModel);
    }
}
