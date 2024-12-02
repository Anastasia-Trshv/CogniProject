using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Cogni.Controllers
{ 
    [ApiController]
    [Route("[controller]/[action]")]
    
    public class CogniController : ControllerBase//временный контроллер
    {
        private readonly CogniDbContext _context;
        public CogniController(CogniDbContext cogniDb)
        {
            _context = cogniDb;
        }
        /// <summary>
        /// Контроллер для тестов бэка, все методы не для фронта
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> CreateRole(string roleName, int id)
        {
            await _context.Roles.AddAsync(new Database.Entities.Role(id, roleName));
            _context.SaveChanges();
            return Ok(id);
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            List<Role> roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateMbti(string Name)
        {
            await _context.MbtiTypes.AddAsync(new MbtiType { NameOfType = Name });
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetMbtiTypes()
        {
            List<MbtiType> types = await _context.MbtiTypes.ToListAsync();
            return Ok(types);
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        //[HttpPost]
        //public async Task<ActionResult> CreateQuestions()
        //{
        //    await _context.MbtiQuestions.AddRangeAsync
        //        ([
        //            new MbtiQuestion { Question = "Вы предпочитаете проводить время в компании людей." },
        //            new MbtiQuestion { Question = "Вам комфортнее работать в одиночку, чем в команде." },
        //            new MbtiQuestion { Question = "Вы предпочитаете проводить вечера дома в тишине." },
        //            new MbtiQuestion { Question = "Вы чувствуете себя энергичным после общения с другими людьми." },
        //            new MbtiQuestion { Question = "Вам нужно время наедине с собой, чтобы восстановить силы." },
        //            new MbtiQuestion { Question = "Вы любите участвовать в общественных мероприятиях и вечеринках." },
        //            new MbtiQuestion { Question = "Вы предпочитаете глубокие разговоры с несколькими близкими друзьями, чем общение с большой группой." },
        //            new MbtiQuestion { Question = "Вы часто делитесь своими мыслями и чувствами с другими." },
        //            new MbtiQuestion { Question = "Вам сложно находить темы для разговора на больших собраниях." },
        //            new MbtiQuestion { Question = "Вы часто задумываетесь о своих мыслях и чувствах в одиночестве." },
        //            new MbtiQuestion { Question = "Вы чувствуете себя комфортно в больших группах людей." },
        //            new MbtiQuestion { Question = "Вы часто ищете возможности для общения и взаимодействия с другими." },
        //            new MbtiQuestion { Question = "Вы предпочитаете проводить время наедине с книгой или фильмом, чем на шумной вечеринке." },
        //            new MbtiQuestion { Question = "Вы регулярно заводите новых друзей." },
        //            new MbtiQuestion { Question = "Вы легко можете завязать разговор с незнакомым человеком." },
        //            //15
        //            new MbtiQuestion { Question = "Вам бывает сложно сосредоточиться на повседневных рутинных задачах." },
        //            new MbtiQuestion { Question = "Вас больше привлекают новые идеи и инновации, чем проверенные временем методы." },
        //            new MbtiQuestion { Question = "Вы чаще мечтаете и фантазируете, чем фокусируетесь на реальных, текущих событиях." },
        //            new MbtiQuestion { Question = "Вы предпочитаете исследовать новые возможности, даже если они абстрактны, а не использовать уже проверенные методы." },
        //            new MbtiQuestion { Question = "Вы склонны больше доверять своим догадкам и предчувствиям, чем конкретным фактам и данным." },
        //            new MbtiQuestion { Question = "Вы часто находите связи между вещами, которые другие могут не заметить." },
        //            new MbtiQuestion { Question = "В разговорах вы часто перескакиваете с одной идеи на другую, вместо того чтобы обсуждать конкретную тему детально." },
        //            new MbtiQuestion { Question = "Вы часто используете аналогии и сравнения для передачи новых идей." },
        //            new MbtiQuestion { Question = "Вы тяготеете к абстрактному и часто одержимы смысловыми значениями чего-либо." },
        //            new MbtiQuestion { Question = "Вы предпочитаете жить в своих мечтах, а не в реальном мире." },
        //            new MbtiQuestion { Question = "Вы доверяете неопровержимым фактам и сведениям больше, чем чему-либо еще." },
        //            new MbtiQuestion { Question = "Вы «просто знаете» вещи, будучи не в состоянии отчетливо выразить их словами." },
        //            new MbtiQuestion { Question = "Вам бывает скучно заниматься повседневными, практическими задачами, если они не содержат чего-то нового или неизвестного." },
        //            new MbtiQuestion { Question = "Вы чувствуете, что интуиция играет большую роль в ваших решениях, чем опыт." },
        //            new MbtiQuestion { Question = "Вам нравится думать о будущем и строить планы, основанные на возможностях, а не на текущих реалиях." },
        //            //15
        //            new MbtiQuestion { Question = "Вам сложно проявлять эмоции на публике. " },
        //            new MbtiQuestion { Question = "Вы предпочитаете прямую и честную критику, даже если она может показаться резкой. " },
        //            new MbtiQuestion { Question = "Когда кто-то делится с вами своими проблемами, вы чаще даете советы, чем просто слушаете и поддерживаете." },
        //            new MbtiQuestion { Question = "Вы считаете, что решение проблемы должно быть рациональным, даже если оно вызовет недовольство у других." },
        //            new MbtiQuestion { Question = "Вы всегда стараетесь быть тактичным в общении с людьми." },
        //            new MbtiQuestion { Question = "Когда вы слышите жалобы или проблемы других людей, вы сразу ищете пути их решения, а не просто выражаете поддержку." },
        //            new MbtiQuestion { Question = "Вы легко сочувствуете чужим трудностям." },
        //            new MbtiQuestion { Question = "Вы предпочитаете решать проблемы через анализ и логику, чем через обсуждение и поиск эмоциональной поддержки." },
        //            new MbtiQuestion { Question = "В сложных ситуациях вы чаще полагаетесь на объективные факты, чем на собственные или чужие чувства." },
        //            new MbtiQuestion { Question = "Вы редко позволяете своим эмоциям влиять на ваше поведение или действия." },
        //            new MbtiQuestion { Question = "Вы считаете, что справедливость важнее сострадания в решении вопросов." },
        //            new MbtiQuestion { Question = "Вы считаете, что лучше решить задачу рационально и быстро, чем тратить время на обсуждение чувств и мнений других людей." },
        //            new MbtiQuestion { Question = "Вы предпочли бы, чтобы ваши решения основывались на фактах, а не на мнениях окружающих." },
        //            new MbtiQuestion { Question = "Вы испытываете затруднения, когда нужно поддержать кого-то эмоционально, вместо того чтобы предлагать решение проблемы." },
        //            new MbtiQuestion { Question = "Вы считаете, что эмоции могут мешать принятию правильных решений." },
        //            //15
        //            new MbtiQuestion { Question = "Вы предпочитаете иметь четкий план на день, чем действовать спонтанно." },
        //            new MbtiQuestion { Question = "Вам комфортнее, когда все ваши дела организованы и упорядочены." },
        //            new MbtiQuestion { Question = "Вы часто откладываете дела на последний момент." },
        //            new MbtiQuestion { Question = "Вы предпочитаете завершать задачи, прежде чем начинать что-то новое." },
        //            new MbtiQuestion { Question = "Вам нравится, когда у вас есть четкие сроки для выполнения задач." },
        //            new MbtiQuestion { Question = "Вы часто планируете свои выходные заранее." },
        //            new MbtiQuestion { Question = "Вам сложно адаптироваться к изменениям в планах." },
        //            new MbtiQuestion { Question = "Вы предпочитаете следовать расписанию, чем импровизировать." },
        //            new MbtiQuestion { Question = "Вы часто чувствуете себя некомфортно, когда не знаете, что будет дальше." },
        //            new MbtiQuestion { Question = "Вам нравится, когда все вещи находятся на своих местах." },
        //            new MbtiQuestion { Question = "Вы предпочитаете завершить проект, прежде чем переходить к следующему." },
        //            new MbtiQuestion { Question = "Вы часто оставляете свои дела незавершенными." },
        //            new MbtiQuestion { Question = "Вам нравится, когда у вас есть возможность менять свои планы в последний момент." },
        //            new MbtiQuestion { Question = "Вы предпочитаете заранее обдумать свои решения, чем действовать на импульсе." },
        //            new MbtiQuestion { Question = "Вы чувствуете себя более продуктивным, когда у вас есть четкий план действий." },
        //            //15
        //            ]
        //         );
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}
        [HttpDelete]
        public async Task DeleteQuestions(int id)
        {
            var q = await _context.MbtiQuestions.FindAsync(id);
            _context.MbtiQuestions.Remove(q);
            await _context.SaveChangesAsync();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTag()
        {
            await _context.Tags.AddRangeAsync(new Tag[]
             {
            new Tag {NameTag="Вязание"},
            new Tag {NameTag="Кино"},
            new Tag {NameTag="Аниме"},
            new Tag {NameTag="Гарри Поттер"},
            new Tag {NameTag="Лыжный спорт"},
            new Tag {NameTag="Коньки"},

             });
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> AddFriend(int id1, int id2)
        {
            await _context.AddAsync(
            
                new Friend{FriendId=id1, UserId = id2, DateAdded=DateTime.Now}
            );
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<List<Friend>> GetAllFriend()
        {
            return await _context.Friends.ToListAsync();
        }
        //[HttpDelete]
        //public async Task RemoveFriend(int id1, int id2)
        //{
        //     var a = await _context.Friends.FindAsync(id);
        //     _context.Friends.Remove(a);
        //     await _context.SaveChangesAsync();
        //}
        [HttpGet]
        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        [HttpDelete]
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
        }
    }
}
