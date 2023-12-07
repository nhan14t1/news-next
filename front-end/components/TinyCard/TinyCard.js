import styles from './TinyCard.module.scss';
import PropTypes from 'prop-types';

function TinyCard({ title, bgColor }) {
  return (
    <div className={styles.wrapper}>
      <div className={styles.title}>
        <p>{title}</p>
      </div>
      <div
        className={styles.baseline}
        style={{ background: `${bgColor || '#d32f2f'}` }}
      ></div>
    </div>
  );
}

TinyCard.propTypes = {
  title: PropTypes.string.isRequired,
  bgColor: PropTypes.string,
};

export default TinyCard;
