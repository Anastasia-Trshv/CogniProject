import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './Navigation.css';
import { Context } from '../../..';

function Navigation() {

  const {store} = useContext(Context);

  const navigate = useNavigate();

  const logout = async () => {
    let response = await store.logout();
    navigate("/login");
  };

  return (
    <nav className="navigation">
        <Link to="/profile" className='navigation__link'>Мой профиль</Link>
        <Link to="/messages" className='navigation__link'>Чаты</Link>
        <Link to="/wiki" className='navigation__link'>Вики</Link>
        <Link to="/settings" className='navigation__link'>Настройки</Link>
        <button onClick={logout} className='navigation__link navigation__link--red'>Выйти</button>
    </nav>
  );
};

export default Navigation;

