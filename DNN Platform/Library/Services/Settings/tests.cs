////using dgzfp.tagung.dnn;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Xunit;
////using FluentAssertions;

////public class StringBasedSettingsTests
////{
////    class SutClass : DotNetNuke.Services.Settings.StringBasedSettings
////    {
////        public SutClass(Func<string, string> get, Action<string, string> set) : base(get, set) { }
////        public SutClass() : base(_get_dummy, _set_dummy) { }

////        public string AString { get { return Get(); } set { Set(value); } }
////        public int AnInt { get { return Get<int>(); } set { Set(value); } }
////        public bool ABool { get { return Get<bool>(); } set { Set(value); } }
////        public DateTime ADate { get { return Get<DateTime>(); } set { Set(value); } }
////    }

////    static Func<string, string> _get_dummy = name => null; //returns null
////    static Action<string, string> _set_dummy = (name, value) => { };//does nothing

////    [Fact]
////    public void Properties_can_be_initialized_from_external_getter()
////    {
////        Func<string, string> identity = name => name;
////        var sut = new SutClass(identity, _set_dummy);
////        sut.AString.Should().Be("AString");
////    }

////    [Fact]
////    public void External_setter_is_called_when_a_property_was_changed_and_saved()
////    {
////        var counter = 0;
////        Action<string, string> _set_counter = (name, value) => counter++;
////        var sut = new SutClass(_get_dummy, _set_counter);
////        sut.AString = "";
////        counter.Should().Be(0);
////        sut.Save();
////        counter.Should().Be(1);
////    }

////    [Fact]
////    public void Changing_multiple_properties_a_few_times_to_different_values_should_only_execute_setter_the_last_ones_on_save()
////    {
////        var counter = 0;
////        Action<string, string> _set_counter = (name, value) => counter++;
////        var sut = new SutClass(_get_dummy, _set_counter);
////        sut.AString = "A";
////        sut.AnInt = 1;
////        sut.AString = "B";
////        sut.AnInt = 2;
////        counter.Should().Be(0);
////        sut.Save();
////        counter.Should().Be(2);
////    }

////    [Fact]
////    public void Updates_to_properties_without_modifications_are_getting_ignored()
////    {
////        var counter = 0;
////        Action<string, string> _set_counter = (name, value) => counter++;
////        var sut = new SutClass(_get_dummy, _set_counter);
////        sut.AString = "A";
////        sut.AnInt = 1;
////        sut.Save();
////        counter.Should().Be(2);
////        sut.AString = "A";
////        sut.AnInt = 2;
////        sut.Save();
////        counter.Should().Be(3);
////    }

////    [Fact]
////    public void External_setter_is_called_only_once_per_change()
////    {
////        var counter = 0;
////        Action<string, string> _set_counter = (name, value) => counter++;
////        var sut = new SutClass(_get_dummy, _set_counter);
////        sut.AString = "";
////        sut.Save();
////        counter = 0;
////        sut.Save();
////        counter.Should().Be(0);
////    }

////    [Fact]
////    public void It_can_set_and_read_string_properties()
////    {
////        var input = "dnn-connect";
////        var sut = new SutClass();
////        sut.AString = input;

////        sut.AString.Should().Be(input);
////    }

////    [Fact]
////    public void It_can_set_and_read_integer_properties()
////    {
////        var input = 1;
////        var sut = new SutClass();
////        sut.AnInt = input;
////        Assert.Equal(input, sut.AnInt);
////    }

////    [Fact]
////    public void It_can_set_and_read_boolean_properties()
////    {
////        var input = true;
////        var sut = new SutClass();
////        sut.ABool = input;
////        Assert.Equal(input, sut.ABool);
////    }

////    [Fact]
////    public void It_can_set_and_read_date_properties()
////    {
////        var input = DateTime.Parse("06/06/2016");
////        var sut = new SutClass();
////        sut.ADate = input;
////        sut.ADate.Should().Be(input);

////    }

////    [Fact]
////    public void It_returns_default_values_if_external_getter_returns_null()
////    {
////        var sut = new SutClass();
////        sut.ADate.Should().Be(default(DateTime));
////        sut.AString.Should().Be(default(string));
////        sut.AnInt.Should().Be(default(int));
////        sut.ABool.Should().Be(default(bool));
////    }
////}
