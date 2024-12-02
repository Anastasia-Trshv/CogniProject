import React, { useContext, useEffect, useState } from 'react';
import './Settings.css';
import { Context } from '../..';

function Settings() {


  const {store} = useContext(Context);

  const [userName, setUserName] = useState(null);
  const [userSurname, setUserSurname] = useState(null);
  const [userDescription, setUserDescription] = useState(null);
  const [userImage, setUserImage] = useState(null);

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
      } catch (error) {
          console.error("Failed to fetch user data:", error);
      } finally {
          setIsLoading(false);
      }
    };

    fetchUserData();
    
  }, [])

  if (isLoading) {
    return <></>;
}

  return (
    <div className='profile__wrapper'>
      <div className="profile__main">
        <section className="profile__bg"></section>
        <section className="profile__human human human--settings">
          <div className="human__left">
            <span src={userImage} className="human__avatar human__avatar--settings"></span>
          </div>
          <div className="human__right human__right--settings">
            <h1 className='human__name'>{userName + " " + userSurname }</h1>
            <p className='human__description'>{userDescription}</p>
          </div>
        </section>
        <section className="settings__mbti">
          <h3 className="settings__mbti__heading">Изменить типа <br></br> личности</h3>
        </section>
      </div>
    </div>
  );
};

export default Settings;

