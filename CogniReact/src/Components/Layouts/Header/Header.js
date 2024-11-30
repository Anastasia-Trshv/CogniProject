import React from 'react';
import { Link } from 'react-router-dom';
import { ReactComponent as LogoSvg } from './img/logo.svg';
import './Header.css';

function Header() {

  return (
    <header className='header'>
      <div className='header__wrapper'>
        <nav className='header__nav'>
          <div className='header__logo-wrapper'>
            <LogoSvg className='header__logo'></LogoSvg>
            <div className='header__links'>
              <p className='header__title'>COGNI</p>
              <Link to="/about" className='header__about'>О сервисе</Link>
            </div>
          </div>
          <input type="search" className='header__search' placeholder='Найти друга'></input>
        </nav>
      </div>
    </header>
  );
};

export default Header;

