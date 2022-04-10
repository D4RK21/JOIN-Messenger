using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class PersonalChatService : IPersonalChatService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public PersonalChatService(ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            this._currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePersonalChat(List<string> userToChatWith)
        {
            var participants = new List<User> {_currentUser.User};

            foreach (var userInPersonalChat in userToChatWith)
            {
                var users = _unitOfWork.UserRepository.FindByCondition(user =>
                    user.UserName == userInPersonalChat || user.Email == userInPersonalChat);
                var user = users.FirstOrDefault();
                if (user != null && !participants.Select(x => x.Id).Contains(user.Id))
                {
                    participants.Add(user);
                }
            }

            if (participants.Count == 1)
            {
                return;
            }

            var chatName = GenerateChatName(userToChatWith);

            var personalChat = new PersonalChat {ChatName = chatName, Participants = new List<UsersPersonalChats>()};


            _unitOfWork.CreateTransaction();

            foreach (var participant in participants)
            {
                var userOfPersonalChat = new UsersPersonalChats() {User = participant, PersonalChat = personalChat};
                personalChat.Participants.Add(userOfPersonalChat);
            }

            try
            {
                await _unitOfWork.PersonalChatRepository.CreateAsync(personalChat);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
            }
            

            // foreach (var userOfPersonalChat in personalChat.Participants)
            // {
            //     await _unitOfWork.UsersPersonalChats.CreateAsync(userOfPersonalChat);
            //     await _unitOfWork.SaveAsync();
            // }


            //await _unitOfWork.PersonalChatRepository.CreateAsync(personalChat);
            //await _unitOfWork.SaveAsync();

            // await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ChangeNameOfPersonalChat(PersonalChat chat, string name)
        {
            chat.ChatName = name;
            
            _unitOfWork.PersonalChatRepository.Update(chat);
            _unitOfWork.Save();
            
            return true;
        }

        public async Task AddParticipantsToPersonalChat(PersonalChat chat, List<string> participants)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                foreach (var participant in participants)
                {
                    var usersOfPersonalChats = new UsersPersonalChats();
                    var user = (_unitOfWork.UserRepository.FindByCondition(user =>
                        user.UserName == participant || user.Email == participant)).FirstOrDefault();

                    if (!chat.Participants.Select(x => x.User.Id).Contains(user.Id))
                    {
                        usersOfPersonalChats.PersonalChat = chat;
                        usersOfPersonalChats.User = user;
                        _unitOfWork.UsersPersonalChats.Update(usersOfPersonalChats);
                    }
                }

                _unitOfWork.PersonalChatRepository.Update(chat);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
        }

        public async Task<IList<PersonalChat>> GetUserPersonalChats()
        {
            // var users = await
            //     _unitOfWork.PersonalChatRepository.FindByConditionAsync(chat => chat.Participants
            //         .Select(x => x.User.Id)
            //         .Contains(_currentUser.User.Id));


            var users = _unitOfWork.PersonalChatRepository.FindByCondition(q => q.Participants
                .Select(chats => chats.User.Id)
                .Contains(_currentUser.User.Id), PersonalChat.Selector);

            return users.ToList();
        }

        public async Task<IList<string>> GetUserNamesOfChat(PersonalChat chat)
        {
            var users = chat.Participants.Select(user => user.User);

            return users.Select(user => user.UserName).ToList();
        }

        public async Task<bool> LeavePersonalChat(PersonalChat chat)
        {
            var user = _currentUser.User;
            //var participant = chat.Participants.FirstOrDefault(chats => chats.User.Id == user.Id);
            var participant = chat.Participants.First(personalChats => personalChats.User.Id == user.Id);

            try
            {
                _unitOfWork.CreateTransaction();

                if (!chat.Participants.Remove(participant))
                {
                    return false;
                }

                if (chat.Participants.Count == 0)
                {
                    _unitOfWork.PersonalChatRepository.Delete(chat);
                    return true;
                }

                _unitOfWork.UsersPersonalChats.Delete(participant);
                _unitOfWork.PersonalChatRepository.Update(chat);
                
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }

            return true;
        }

        private string GenerateChatName(List<string> userNames)
        {
            var chatName = new StringBuilder();

            for (var i = 0; i < userNames.Count && i < 5; i++)
            {
                chatName.Append(userNames[i]);
                if (i != 4 && i != userNames.Count - 1)
                {
                    chatName.Append(',');
                }
            }

            return chatName.ToString();
        }
    }
}
