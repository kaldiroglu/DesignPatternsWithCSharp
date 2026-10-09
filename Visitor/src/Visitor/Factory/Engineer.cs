namespace dev.kaldiroglu.Visitor.Factory;

public class Engineer : Employee
{
    private readonly string project;

    public Engineer(int no, string name, int year, string department, string project)
        : base(no, name, year, department)
    {
        this.project = project;
    }

    public override void Work()
    {
        Console.WriteLine("Engineer is working on a project: " + project);
    }

    public void AssignTask(string task)
    {
        Console.WriteLine("Engineer " + Name + " works on the task: " + task);
    }
}
