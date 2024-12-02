import React, { useContext, useEffect, useState } from 'react';
import './Profile.css';
import { Context } from '../..';

function Profile() {


  const {store} = useContext(Context);

  const [userName, setUserName] = useState(null);
  const [userSurname, setUserSurname] = useState(null);
  const [userDescription, setUserDescription] = useState(null);
  const [userImage, setUserImage] = useState(null);
  const [userTypeMBTI, setUserTypeMBTI] = useState(null);

  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = async () => {
      setIsLoading(true);
      const userId = localStorage.userId; 
      

      try {
        const userInfo = await store.userInfo(userId);
        setUserName(userInfo.name);
        setUserSurname(userInfo.surname);
        setUserDescription(userInfo.description);
        setUserImage(userInfo.image);
        setUserTypeMBTI(userInfo.typeMbti);
      } catch (error) {
          console.error("Failed to fetch user data:", error);
      } finally {
          setIsLoading(false);
      }
    };

    const fetchUserPosts = async () => {
      setIsLoading(true);
      const userId = localStorage.userId; 

      try {
        const userPosts = await store.getPosts(userId);
        console.log(userPosts);
      } catch (error) {
          console.error("Failed to fetch user posts:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserData();
    fetchUserPosts();
    
  }, [])

  if (isLoading) {
    return <></>;
}

  return (
    <div className='profile__wrapper'>
      <div className="profile__main">
        <section className="profile__bg"></section>
        <section className="profile__human human">
          <div className="human__left">
            <span src={userImage} className="human__avatar"></span>
            <span className="human__mbti">{userTypeMBTI}</span>
          </div>
          <div className="human__right">
            <h1 className='human__name'>{userName + " " + userSurname }</h1>
            <p className='human__description'>{userDescription}</p>
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
        <section className='profile__posts posts'>
          <ul>
            <li>
              <div className='posts__author'>
                <img className='posts__avatar'></img>
                <div className='posts__info'>
                  <p className='posts__name'></p>
                  <span className='posts__time'></span>
                </div>
              </div>
              <div className='posts__post post'>
                  <p className='post__description'></p>
                  <img></img>
              </div>
            </li>
          </ul>
        </section>
      </div>
      <div className="profile__addons addons">
        <section className='addons__friends'></section>
        <section className='addons__more'></section>
      </div>
    </div>
  );
};

export default Profile;

