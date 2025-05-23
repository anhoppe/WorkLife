using WorkLife.Model.Contract;

namespace WorkLife.Model
{
    internal class PersonBuilder : IPersonBuilder
    {
        private IDataProvider _dataProvider;
        
        private Person _person;

        public PersonBuilder(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _person = new Person(_dataProvider);
        }


        public IPerson Build()
        {
            _person.AppliedTargetTime = _person.AppliedTargetTime.OrderBy(p => p.Item1).ToList();
            var createdPerson = _person;

            // Create a new person to make sure the builder is reusable
            _person = new Person(_dataProvider);

            return createdPerson;
        }

        public IPersonBuilder WithEmployeeId(string employeeId)
        {
            _person.EmployeeId = employeeId;
            return this;
        }

        public IPersonBuilder WithId(int id)
        {
            _person.Id = id;
            return this;
        }

        public IPersonBuilder WithName(string name)
        {
            _person.Name = name;
            return this;
        }

        public IPersonBuilder WithTargetTimeModel(DateOnly startDate, TargetTimeModel targetTimeModel)
        {
            _person.AppliedTargetTime.Add((startDate, targetTimeModel));
            return this;
        }
    }
}
