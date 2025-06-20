﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpaceBattle.Tests
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class СозданиеМакрокоммандыFeature : object, Xunit.IClassFixture<СозданиеМакрокоммандыFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Macro.feature"
#line hidden
        
        public СозданиеМакрокоммандыFeature(СозданиеМакрокоммандыFeature.FixtureData fixtureData, SpaceBattle_Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru"), "", "Создание макрокомманды", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Макрокомманда создалась успешно")]
        [Xunit.TraitAttribute("FeatureTitle", "Создание макрокомманды")]
        [Xunit.TraitAttribute("Description", "Макрокомманда создалась успешно")]
        public void МакрокоммандаСоздаласьУспешно()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Макрокомманда создалась успешно", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
    testRunner.Given("Имя составной операции и объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 7
    testRunner.When("Регистрация комманд в IoC для создания макрокомманды", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 8
    testRunner.Then("Выполнение макрокоммады и проверка вызовов комманды", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Выполнение макрокомманды")]
        [Xunit.TraitAttribute("FeatureTitle", "Создание макрокомманды")]
        [Xunit.TraitAttribute("Description", "Выполнение макрокомманды")]
        public void ВыполнениеМакрокомманды()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Выполнение макрокомманды", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 10
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 11
    testRunner.Given("Настройка имитированной комманды и выполнение макрокомманды с несколькими комманд" +
                        "ами", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 12
    testRunner.When("Создание и выполнение макрокомманды", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 13
    testRunner.Then("Проверка вызовов имитированной команды", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Выброс исключения")]
        [Xunit.TraitAttribute("FeatureTitle", "Создание макрокомманды")]
        [Xunit.TraitAttribute("Description", "Выброс исключения")]
        public void ВыбросИсключения()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Выброс исключения", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 15
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 16
    testRunner.Given("Комманды вызывающей исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 17
    testRunner.When("Попытка выполнения ее в макрокомманде", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 18
    testRunner.Then("Проверка что выполнение комманды в макрокомманде вызывает исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                СозданиеМакрокоммандыFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                СозданиеМакрокоммандыFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
