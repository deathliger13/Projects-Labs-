Template.Chat.helpers({
  messages: function () {
    return Messages.find({}, { sort: { createdAt: 1 } });
  },
  users: function () {
    return Meteor.users.find();
  }
});
