using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace eduProjectTests.RepositoryTests
{
    public class UserSettingsRepositoryTests
    {
        private readonly UserSettingsRepository settings;

        public UserSettingsRepositoryTests()
        {
            settings = new UserSettingsRepository(new TestDbConnectionParameters());

            Tag.tags.Clear();

            Tag.tags.Add(1, new Tag { Name = "web" });
            Tag.tags.Add(2, new Tag { Name = "desktop" });
            Tag.tags.Add(3, new Tag { Name = "mobile" });
            Tag.tags.Add(4, new Tag { Name = "iot" });


        }

        [Fact]
        public async void GetById_IdExists_ReturnUserSettings()
        {
            var result = await settings.GetAsync(1);
            Assert.IsType<UserSettings>(result);
            var bio = result.Bio;
            Assert.NotNull(bio);


        }

        [Fact]
        public async void GetById_IdIsNonExisting_ReturnNull()
        {
            var result = await settings.GetAsync(0);
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateTags_UserTagsExist_TagListUpdated()
        {
            var s = await settings.GetAsync(1);
            s.UserTags.Remove(Tag.tags[1]);
            Assert.Equal(1, s.UserTags.Count);
            s.UserTags.Add(Tag.tags[4]);
            Assert.Equal(2, s.UserTags.Count);
        }
    }
}
