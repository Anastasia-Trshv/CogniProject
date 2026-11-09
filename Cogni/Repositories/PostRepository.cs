using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;
using Cogni.Database.Entities;

namespace Cogni.Database.Repositories;
public class PostRepository : IPostRepository
{
    private readonly CogniDbContext _context;
    public PostRepository(CogniDbContext context)
    {
        _context = context;
    }
    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task DeletePost(int id)
    {
        Post post = await _context.Posts.FindAsync(id);
        if (post is null)
        {
            return;
        }
        _context.Remove(post);
        await _context.SaveChangesAsync();
    }


    public async Task ToggleReactionAsync(
    int postId,
    int userId,
    string reactionType = "like")
    {
        var existingReaction = await _context.Set<Like>()
            .FirstOrDefaultAsync(r =>
                r.PostId == postId &&
                r.UserId == userId);

        if (existingReaction is null)
        {
            var reaction = new Like
            {
                PostId = postId,
                UserId = userId,
                ReactionType = reactionType
            };

            await _context.Set<Like>().AddAsync(reaction);
        }
        else if (existingReaction.ReactionType == reactionType)
        {
            _context.Set<Like>().Remove(existingReaction);
        }
        else
        {
            existingReaction.ReactionType = reactionType;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Post>> GetAllUserPosts(int id)
    {
        var posts = await _context.Posts
            .Include(a => a.PostImages)
            .Where(u => u.IdUser == id)
            .ToListAsync();
        return posts;
    }

    public async Task<Post> UpdatePost(Post post)//TODO
    {
        Post up = new Post { Id = post.Id , IdUser = post.IdUser, PostBody = post.PostBody, PostImages = post.PostImages};
        await _context.SaveChangesAsync();
        return up;

    }

    public async Task<Post?> GetPostById(int id)
    {
        return await _context.Posts.FindAsync(id);
    }


    public async Task<(int ReactionsCount, bool IsReactedByCurrentUser)> GetReactionInfoAsync(
    int postId,
    int userId)
    {
        var reactions = _context.Likes
            .Where(r => r.PostId == postId);

        int reactionsCount = await reactions.CountAsync();

        bool isReactedByCurrentUser = await reactions
            .AnyAsync(r => r.UserId == userId);

        return (reactionsCount, isReactedByCurrentUser);
    }

}

