Template.ChatMessage.helpers({
  author: function () {
    return Meteor.users.findOne({ _id: this.userId });
  },
  timeFromNow: function() {
    return moment(this.createdAt).fromNow();
  },
  floatСlass: function () {
    return Meteor.userId() === this.userId ? 'right' : 'left'
  }
});

Template.ChatMessage.onRendered(function () {
  var self = this;
  var $scrollable = self.$('.chat-message').closest('.chat-discussion');
  var $messages = self.$('.chat-message').closest('.messages-wrapper');
  $scrollable.scrollTop($messages.height());
});