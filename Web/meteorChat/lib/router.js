Router.configure({
  loadingTemplate: 'spinner'
});

Router.route('/', {
  name: 'home',
  template: 'Chat',
  waitOn: function () {
    return [Meteor.subscribe('users'), Meteor.subscribe('messages')];
  }
});
Router.route('/per', {
  name: 'media',
  template: 'media',
  //waitOn: function () {
    //return [Meteor.subscribe('users'), Meteor.subscribe('messages')];
  });
