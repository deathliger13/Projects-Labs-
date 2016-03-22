Meteor.publish('users', function () {
  return Meteor.users.find({}, { fields: { color: 1, username: 1 } });
});

Meteor.publish('messages', function () {
  return Messages.find();
});