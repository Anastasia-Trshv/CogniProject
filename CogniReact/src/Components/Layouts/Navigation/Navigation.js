import React from 'react';
import { Link } from 'react-router-dom';
import './Navigation.css';

function Navigation() {

  return (
    <nav className="navigation">
        <Link to="/profile" className='navigation__link'>Мой профиль</Link>
        <Link to="/messages" className='navigation__link'>Чаты</Link>
        <Link to="/wiki" className='navigation__link'>Вики</Link>
        <Link to="/settings" className='navigation__link'>Настройки</Link>
    </nav>
  );
};

export default Navigation;

