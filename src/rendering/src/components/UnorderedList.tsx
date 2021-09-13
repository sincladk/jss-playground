import { Text, Field, Placeholder } from '@sitecore-jss/sitecore-jss-nextjs';
import { StyleguideComponentProps } from 'lib/component-props';

type UnorderedListProps = StyleguideComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const UnorderedList = (props: UnorderedListProps): JSX.Element => (
  <div>
    <Text tag="h3" field={props.fields.heading} />
    <ul>
      <Placeholder name="jss-list-items" rendering={props.rendering} 
        renderEach={(component, index) => (
          <div className="col-sm" key={index}>
            {component}
          </div>
        )}
      />
    </ul>
  </div>
);

export default UnorderedList;
