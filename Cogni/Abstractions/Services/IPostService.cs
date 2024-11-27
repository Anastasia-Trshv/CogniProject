﻿using Cogni.Contracts.Requests;
using Cogni.Models;

namespace Cogni.Abstractions.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllUserPosts(int id);
        Task<Post> CreatePost(PostRequest post);
        //Task<Post> UpdatePost(PostRequest post);
        Task DeletePost(int id);
    }
}