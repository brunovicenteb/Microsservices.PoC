﻿using Toolkit.Data;

namespace Toolkit.Test.Data;

public class BaseEntityTest
{
    #region NewBaseEntityTest
    private class NewBaseEntityTest : BaseEntity
    {
        public NewBaseEntityTest() :
            base()
        {
        }
        public NewBaseEntityTest(string id) :
            base(id)
        {
        }
    }
    #endregion

    [Theory]
    [InlineData("0")]
    [InlineData("25")]
    [InlineData("43")]
    public void CreatorTest(string id)
    {
        var newBaseEntity = new NewBaseEntityTest(id);
        Assert.Equal(id, newBaseEntity.ID);
    }

    [Fact]
    public void GetValidatorsTest()
    {
        var newBaseEntity = new NewBaseEntityTest();
        var validators = newBaseEntity.GetValidators();
        Assert.NotNull(validators);
        Assert.Empty(validators);
    }
}