using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
using TechTalk.SpecFlow;

namespace SpaceshipTests.MacroCMD
{
    [Binding]
    public class TestCreateMacroCommand
    {
        private Mock<ShipNamespace.IComand>? mockCommand;
        private string? operationName;
        private Mock<UniversalyObject>? mockUObject;

        private List<ShipNamespace.IComand>? commands;
        public static void InitScope()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }
        [Given(@"Имя составной операции и объект")]
        public void GivenNameAndObj()
        {
            InitScope();
            mockCommand = new Mock<ShipNamespace.IComand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            operationName = "Movement";
            mockUObject = new Mock<UniversalyObject>(new Dictionary<string, object>());
            mockUObject.Object.properties.Add("Position", new Vector2d(0, 0));
        }

        [When(@"Регистрация комманд в IoC для создания макрокомманды")]
        public void GivenImpossibleAngleSpeed()
        {
            Assert.NotNull(mockCommand);
            Assert.NotNull(mockUObject);
            Assert.NotNull(operationName);
            IoC.Resolve<ICommand>("IoC.Register", "Config." + operationName, (object[] args) => new List<string> { "Game.Command.Move" }).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro.Create",
            (object[] args) => new MacroStrategy().Strat(args[0], args[1])).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Move", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<ShipNamespace.IComand>("Game.Command.Macro.Create", operationName, mockUObject.Object).Execute();
        }

        [Then(@"Выполнение макрокоммады и проверка вызовов комманды")]
        public void GivenImpossibleAngleAction()
        {
            Assert.NotNull(mockCommand);
            mockCommand.VerifyAll();
        }

        [Given(@"Настройка имитированной комманды и выполнение макрокомманды с несколькими коммандами")]
        public void GivenParameters()
        {
            InitScope();
            mockCommand = new Mock<ShipNamespace.IComand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();
        }

        [When(@"Создание и выполнение макрокомманды")]
        public void Running()
        {
            Assert.NotNull(mockCommand);
            new MacroCommand(
            new List<ShipNamespace.IComand> { mockCommand.Object, mockCommand.Object, mockCommand.Object }).Execute();
        }

        [Then(@"Проверка вызовов имитированной команды")]
        public void CheckCommand()
        {
            Assert.NotNull(mockCommand);
            mockCommand.Verify(x => x.Execute(), Times.Exactly(3));
        }

        [Given(@"Комманды вызывающей исключение")]
        public void GivenCommandWithEx()
        {
            InitScope();
            mockCommand = new Mock<ShipNamespace.IComand>();
            mockCommand.Setup(x => x.Execute()).Throws(new Exception());

        }

        [When(@"Попытка выполнения ее в макрокомманде")]
        public void TryRunInMacro()
        {
            Assert.NotNull(mockCommand);
            commands = new List<ShipNamespace.IComand> { mockCommand.Object };
        }

        [Then(@"Проверка что выполнение комманды в макрокомманде вызывает исключение")]
        public void CheckEx()
        {
            Assert.NotNull(mockCommand);
            Assert.NotNull(commands);
            Assert.Throws<Exception>(() => new MacroCommand(commands).Execute());
        }
    }
}

