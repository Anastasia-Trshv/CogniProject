import React from 'react';
import './Profile.css';

function Profile() {

  return (
    <div className='profile__wrapper'>
      <div className="profile__main">
        <section className="profile__bg"></section>
        <section className="profile__human human">
          <div className="human__left">
            <span className="human__avatar"></span>
            <span className="human__mbti">INFP</span>
          </div>
          <div className="human__right">
            <h1 className='human__name'>Катя Батурина</h1>
            <p className='human__description'>Toss your dirty shoes in my washing machine heart baby bang it up inside</p>
          </div>
        </section>
        <section className='profile__hobbies hobbies'>
          <h2 className='hobbies__heading'>Увлечения</h2>
          <ul className="hobbies__list">
            <li className='hobbies__item'>#рисование</li>
            <li className='hobbies__item'>#рок</li>
            <li className='hobbies__item'>#рисование</li>
            <li className='hobbies__item'>#пение</li>
            <li className='hobbies__item'>#танцы</li>
          </ul>
        </section>
        <section className='profile__addpost addpost'>
          <h2 className='addpost__heading'>Публикации</h2>
          <button className='addpost__button'>+ новая публикация</button>
        </section>
        {/* <section className='profile__posts posts'>
          <div className='posts__author'>
            <img className='posts__avatar'></img>
            <div className='posts__info'>
              <p className='posts__name'>Катя батурина</p>
              <span className='posts__time'>3 минуты назад</span>
            </div>
            <div className='posts__post post'>
              <p className='post__description'></p>
              <img></img>
            </div>
          </div>
        </section> */}
      </div>
      <div className="profile__addons addons">
        <section className='addons__friends'></section>
        <section className='addons__more'></section>
      </div>
    </div>
  );
};

export default Profile;

