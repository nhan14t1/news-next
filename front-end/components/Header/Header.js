import Image from 'next/legacy/image';
import Link from 'next/link';
import styles from './Header.module.scss';
import { SearchBar } from '../index';

function Header(props) {
  return (
    <header className={styles.header}>
      <div className={`${styles.container} container`}>
        <div className={styles.logo}>
          <Link href='/' onClick={props.onClick}>
            <img src='/assets/logo.svg' alt='Showbiz365 logo' style={{width: 'auto', height: '36px'}} />
            {/* <Image
              src='/assets/logo.svg'
              alt='Showbiz365 logo'
              width={50}
              height={40}
              style={{width: '150px', height: '25px'}}
            /> */}
          </Link>
        </div>
        {/* <div className={styles.search}>
          <SearchBar {...props} />
        </div> */}
      </div>
    </header>
  );
}

export default Header;
