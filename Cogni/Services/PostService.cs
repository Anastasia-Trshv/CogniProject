using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Models;
using Cogni.Database.Entities;

namespace Cogni.Services;
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IImageService _imageService;
    public PostService(IPostRepository postRepository, IImageService imageService)
    {
        _postRepository = postRepository;
        _imageService = imageService;   
    }

    public async Task<Post> CreatePost(PostRequest post, int userid)
    {
        var p = new Post { IdUser = userid, PostBody = post.PostBody};
        p.CreatedAt = DateTime.UtcNow;

        //TODO отправка картинки на облако и получение ссылки
        if(post.Files!= null) 
        {
            foreach (var i in post.Files)
            {
                p.PostImages.Add(new PostImage { ImageUrl = await _imageService.UploadImage(i) });
            }
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

