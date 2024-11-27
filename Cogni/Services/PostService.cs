using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Models;

namespace Cogni.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post> CreatePost(PostRequest post)
        {
            var p = new Post { Id = post.Id, IdUser = post.IdUser, PostBody = post.PostBody};
            //TODO отправка картинки на облако и получение ссылки
            foreach (var i in post.PostImages)
            {
                p.PostImages.Add(new PostImage { ImageUrl = "https://cache3.youla.io/files/images/780_780/5f/09/5f09f7160d4c733205084f38.jpg" });
            }
               
           return await _postRepository.CreatePost(p);

        }

        public async Task DeletePost(int id)
        {
            await _postRepository.DeletePost(id);
        }

        public async Task<List<Post>> GetAllUserPosts(int id)
        {
            var posts = await _postRepository.GetAllUserPosts(id);
            return posts;
        }


        //TODO доделать + облако
        //public async Task<Post> UpdatePost(PostRequest post)
        //{
        //    return await _postRepository.UpdatePost(new Post 
        //    { 
        //        Id = post.Id,
        //        IdUser=post.IdUser, 
        //        PostBody = post.PostBody
        //    });
        //}
    }
}
